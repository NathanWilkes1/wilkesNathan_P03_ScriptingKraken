using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavBrain : MonoBehaviour
{
    [SerializeField] private Transform Target;
    private NavMeshAgent navMeshAgent;
    private Transform Player;
    private bool spotted = false;
    private int patrolCounter = 0;



    enum options_1 // your custom enumeration
    {
        Idle,
        Approach,
        Patrol,
        Roam
    };
    enum options_2 // your custom enumeration
    {
        Idle,
        Approach,
        Roam
    };
    [Header("AI modes")]
    [SerializeField] options_1 Unseen;
    [SerializeField] options_2 Seen;
    [Header("Vision Setting")]
    [SerializeField] [Range(1, 360)] float visionCone = 45f;
    [SerializeField] float visionRange = 10f;
    [SerializeField] [Tooltip("when checked, the AI will be able to look through walls to find their target")] private bool XrayVision = false;
    [Header("Movement")]
    [SerializeField] float MovementSpeed = 1f;
    [SerializeField] [Tooltip("Controls how close the AI gets to their destination")] private float stop = 3;
    private options_1 mode;
    [SerializeField] [Tooltip("Used for the patrol mode in options 1. place empty gameobjects with transforms here to move between")] private Transform[] PatrolPoints;
    

    private void OnDrawGizmosSelected()
    {
        var coneGizmoRay = this.gameObject.transform.position;
        Gizmos.DrawWireSphere(this.transform.position, visionRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, visionCone+ this.gameObject.transform.rotation.eulerAngles.y, 0) * new Vector3(0, 0, visionRange));
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, visionCone*-1+ this.gameObject.transform.rotation.eulerAngles.y, 0) * new Vector3(0, 0, visionRange));



    }




    // Start is called before the first frame update
    private void Start()
    {
        gameObject.AddComponent(typeof(NavMeshAgent));
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 2;
        navMeshAgent.speed = MovementSpeed;
        if(Unseen == options_1.Patrol)
        {
            navMeshAgent.SetDestination(PatrolPoints[0].position);
        }
    }

    private void Update()
    {
        //there might be a more efficient way to write this but for my purposes having the 8 if branches was easy to view and mess with.

        SeekTarget();
        if (spotted == false)
        {
            Debug.Log("unseen");
        if (Unseen == options_1.Idle)
            {
                Debug.Log("unseen idle");
                this.navMeshAgent.Stop();
            }
        else if(Unseen == options_1.Approach)
            {
                Debug.Log("unseen approach");
                this.navMeshAgent.SetDestination(Target.position);
            }
        else if (Unseen == options_1.Patrol)
            {
                Patrolling();
            }
        else if (Unseen == options_1.Roam)
            {
                if(navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    Vector3 newDestination;
                        if(RandomPoint(transform.position,10,out newDestination))
                    {
                        navMeshAgent.SetDestination(newDestination);
                    }
                }
            }
        }
    else
     {
            Debug.Log("seen");
            if (Seen == options_2.Idle)
            {
                Debug.Log("seen idle");
                this.navMeshAgent.Stop();
            }
            else if (Seen == options_2.Approach)
            {
                Debug.Log("seen approach");
                this.navMeshAgent.SetDestination(Target.position);
            }
            else if (Seen == options_2.Roam)
            {
                if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
                {
                    Vector3 newDestination;
                    if (RandomPoint(transform.position, 10, out newDestination))
                    {
                        navMeshAgent.SetDestination(newDestination);
                    }
                }
            }
        }
     

    }

    //roaming code Start
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        //this code finds a random point within a sphere defined by the range.
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        result = Vector3.zero;
        return false;
    }
    //Roaming code End

    //patrol code start
    private void Patrolling()
    {
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            patrolCounter++;
            if(patrolCounter == PatrolPoints.Length)
            {
                patrolCounter = 0;
            }
            navMeshAgent.SetDestination(PatrolPoints[patrolCounter].position);
        }
    }
    //patrol code end
    
    //vision code start.
    private void SeekTarget() 
    {
        //subtracting to vectors from each other gives the line/angle between them
        Vector3 direction = Target.transform.position - transform.position;
        //mathf.abs is used because i want the AI to look in the negative direction too and it's easier to code without negatives.
        if (Mathf.Abs(Vector3.Angle(transform.forward, direction)) < visionCone && Vector3.Distance(Target.transform.position, gameObject.transform.position) < visionRange)
        {

            //I actually took this from my turret script back from project 2.
            ////Raycasting to make sure it's not behind a wall made sense for the AI.
            if (XrayVision == true)
            {
                spotted = true;
            }
            else { 
            Vector3 rayStartPos = gameObject.transform.position;
            Vector3 rayDirection = Target.transform.position - transform.position;

            Debug.DrawRay(rayStartPos, rayDirection * 30, Color.cyan, 1);
            if (Physics.Raycast(rayStartPos, rayDirection, out RaycastHit hitInfo, 30, ~2))
            {


                if (hitInfo.collider.gameObject == Target.gameObject)
                {
                    spotted = true;

                    //I added this since the idle modes would set the navAgent to stop
                    navMeshAgent.Resume();
                }
                else
                {

                }
            }
        }
        }
    }
    //vision code end
}
