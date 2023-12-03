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
    
    [SerializeField] options_1 Unseen;
    [SerializeField] options_2 Seen;
    [SerializeField] [Range(1, 360)] float visionCone = 45f;
    [SerializeField] float visionRange = 10f;
    [SerializeField] bool Forgetful;
    [SerializeField] float AttentionSpanSeconds = 5f;
    [SerializeField] float MovementSpeed = 1f;
    private float stop = 1;
    private options_1 mode;


    private void Awake()
    {
       
        
    }

    // Start is called before the first frame update
    private void Start()
    {
        gameObject.AddComponent(typeof(NavMeshAgent));
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = 2;
    }

    private void Update()
    {
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

            }
        else if (Unseen == options_1.Roam)
            {

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

            }
        }
     

    }

    //roaming code Start
    //This code pretty much comes straight from the unity API as it does exactly what i needed straight out of the box. Changing it would be inefficient.
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
    //Roaming code End
    
    
    //vision code.
    private void SeekTarget() 
    {
        Vector3 direction = Target.transform.position - transform.position;
        if (Mathf.Abs(Vector3.Angle(transform.forward, direction)) < visionCone && Vector3.Distance(Target.transform.position, gameObject.transform.position) < visionRange)  
        {

            //I actually took this from my turret script back from project 2.
            ////Raycasting to make sure it's the right target makes sense to me.

            Vector3 rayStartPos = gameObject.transform.position;
            Vector3 rayDirection = Target.transform.position - transform.position;

            Debug.DrawRay(rayStartPos, rayDirection * 30, Color.cyan, 5);
            if (Physics.Raycast(rayStartPos, rayDirection, out RaycastHit hitInfo, 30, ~2))
            {
                Debug.Log(hitInfo.collider.gameObject.layer);
                Debug.Log(hitInfo.collider.gameObject);

                if (hitInfo.collider.gameObject == Target.gameObject)
                {
                    spotted = true;
                    navMeshAgent.Resume();
                }
                else
                {
                    
                }
            }
        }
    }
}
