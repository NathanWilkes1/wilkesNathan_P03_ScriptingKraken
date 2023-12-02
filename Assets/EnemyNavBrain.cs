using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavBrain : MonoBehaviour
{
    [SerializeField] private Transform Target;
    private NavMeshAgent navMeshAgent;
    private Transform Player;
    

    // Start is called before the first frame update
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
     navMeshAgent.destination = Target.position;

    }
    }
