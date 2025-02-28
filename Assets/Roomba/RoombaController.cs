using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoombaController : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isActive = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;  // Inicialmente parado
    }

    void Update()
    {
        if (isActive && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            RoombaAIBehavior.Instance.SetNewDestination();
        }
    }

    [ContextMenu("Activate")]
    public void ActivateRoomba()
    {
        isActive = true;
        agent.isStopped = false;
        RoombaAIBehavior.Instance.SetNewDestination();
    }

}
