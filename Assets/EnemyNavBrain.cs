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
    [SerializeField] bool Forgetful;
    [SerializeField] float AttentionSpanSeconds = 5;
    [SerializeField] float MovementSpeed = 1;
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
    }

    private void Update()
    {
        if (spotted = false)
        {
        if (Unseen == options_1.Idle)
            {

            }
        else if(Unseen == options_1.Approach)
            {
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
            if (Seen == options_2.Idle)
            {
                this.navMeshAgent.Stop();
            }
            else if (Seen == options_2.Approach)
            {
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


}
