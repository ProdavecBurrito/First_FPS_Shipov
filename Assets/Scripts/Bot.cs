using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[RequireComponent(typeof(NavMeshAgent))]
public class Bot : Unit
{
    NavMeshAgent agent;
    Transform playerTransform;
    Transform target;

    // Поля для проверки, на земле ли бот
    float groundCheckDistance = 0.1f;
    bool grounded;

    float stopDistance = 0.1f;
    float fireDistance = 10f;
    float persuingDistance = 3f;

    [SerializeField] List<Vector3> patrolPoints = new List<Vector3>();
    int wayCounter;
    
    GameObject mainWayPoint;

    float timeToWait = 3f;
    float timeOut;

    [SerializeField] bool patrol;
    [SerializeField] bool shoot;
    [SerializeField] bool aggressionMode;

    [SerializeField] List<Transform> visibleTargets = new List<Transform>();

    protected override void Awake()
    {
        base.Awake();
        Health = 100;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        agent.updateRotation = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        mainWayPoint = FindObjectOfType<TracingWayPoints>().gameObject;// переделать

        foreach (Transform point in mainWayPoint.transform)
        {
            patrolPoints.Add(point.position);
        }
        patrol = true;
        agent.stoppingDistance = stopDistance;
    }
    void Update()
    {
        if (agent)
        {
            if (IsDead)
            {
                Anim.SetBool("Die", true);
                agent.ResetPath();
                RigBody.isKinematic = true;
                Destroy(Instance, 3f);
            }
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                Anim.SetBool("Move", true);
            }
            else
            {
                Anim.SetBool("Move", false);
            }

            if (patrol)
            {
                if (patrolPoints.Count > 1)
                {
                    agent.stoppingDistance = stopDistance;
                    agent.SetDestination(patrolPoints[wayCounter]);
                    if (!agent.hasPath)
                    {
                        timeOut += Time.deltaTime;
                        if (timeOut > timeToWait)
                        {
                            timeOut = 0;
                            if (wayCounter < patrolPoints.Count - 1)
                            {
                                wayCounter++;
                            }
                            else
                            {
                                wayCounter = 0;
                            }
                        }
                    }
                }
                else
                {
                    agent.SetDestination(playerTransform.position);
                    agent.stoppingDistance = fireDistance;
                }
            }
            else
            {
                agent.stoppingDistance = fireDistance;
                agent.SetDestination(target.position);
            }
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + (Vector3.up * groundCheckDistance), Vector3.down, out hit, 0.5f))
        {
            grounded = true;
        }
        else
        {
            grounded = false;
        }
    }
}
