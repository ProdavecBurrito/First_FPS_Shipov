using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : Unit
{
    NavMeshAgent agent;
    Transform playerTransform;
    Transform target;

    float stopDistance = 2f;

    protected override void Awake()
    {
        base.Awake();
        Health = 100;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        agent.updateRotation = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update()
    {
        if (agent)
        {
            agent.SetDestination(playerTransform.position);
            agent.stoppingDistance = stopDistance;
        }
        if (IsDead)
        {
            Destroy(Instance, 5f);
        }
    }
}
