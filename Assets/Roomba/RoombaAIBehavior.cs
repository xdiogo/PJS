using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RoombaAIBehavior : MonoBehaviour
{
    public static RoombaAIBehavior Instance;
    public Transform[] waypoints;
    private NavMeshAgent agent;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void SetNewDestination()
    {
        if (waypoints.Length == 0) return;
        int randomIndex = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[randomIndex].position);
    }
}
