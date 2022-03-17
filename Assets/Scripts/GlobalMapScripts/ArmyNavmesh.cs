using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class ArmyNavmesh : MonoBehaviour
{
    float movementPoints = 100, movementPointsMax = 100;
    private NavMeshAgent navMeshAgent;
    
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    public void SetDestination()
    {

    }
}