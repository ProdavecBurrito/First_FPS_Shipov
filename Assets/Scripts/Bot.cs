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

    [Header("Настройка / Параметры оружия")]
    [SerializeField] Transform fireStart;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject hitParticle;
    [Range(0, 100)][SerializeField] int ammo;
    [Range(100, 1000)][SerializeField] float hitDistance;
    //[ContextMenuItem("Рандомное значение урона", nameof(RandomDmg))]
    [Range(5, 50)][SerializeField] int dmg;

    // Поля для проверки, на земле ли бот
    float groundCheckDistance = 0.1f;
    bool grounded;

    float stopDistance = 0.1f;
    float fireDistance = 6f;
    float persuingDistance = 3f;

    [Header("Зона патрулирования")]
    [SerializeField] List<Vector3> patrolPoints = new List<Vector3>();
    int wayCounter;
    [SerializeField] GameObject mainWayPoint; // Я не уверен, что это верный выход из такой ситуации (когда есть несколько путей)

    float timeToWait = 3f;
    float timeOut;

    [Header("Состояния")]
    [SerializeField] bool patrol;
    [SerializeField] bool shoot;
    [SerializeField] bool aggression;
    [SerializeField] bool findPlayer;

    [Header("Настройки видимости бота")]
    [SerializeField] List<Transform> visibleTargets = new List<Transform>();
    //[ContextMenuItem("Рандомный угол обзора бота", nameof(RandomAngle))]
    [Range(20, 90)][SerializeField] float maxAngle = 30;
    //[ContextMenuItem("Рандомный радиус обзора бота", nameof(RandomRad))]
    [Range(5, 50)][SerializeField] float maxRadius = 20;
    [SerializeField] LayerMask targetMask;
    [SerializeField] LayerMask obstacleMask;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        // Поднятие точки регистрации на 1 юнит
        Vector3 pos =VectorUp();
        // Отрисовка арки
        Handles.color = new Color(1, 0.5f, 0.5f, 0.5f);
        Handles.DrawSolidArc(pos, Vector3.up, Vector3.forward, maxAngle, maxRadius);
        Handles.DrawSolidArc(pos, Vector3.up, Vector3.forward, -maxAngle, maxRadius);
    }

    public void RandomAngle()
    {
        maxAngle = Random.Range(20, 90);
    }

    public void RandomRad()
    {
        maxRadius = Random.Range(5, 50);
    }

    public void RandomDmg()
    {
        dmg = Random.Range(5, 50);
    }

    [ContextMenu("Tools / Значения по умолчанию")]
    void Default()
    {
        ammo = 30;
        dmg = 20;
        hitDistance = 500;
        maxAngle = 30;
        maxRadius = 20;
    }
#endif

    protected override void Awake()
    {
        base.Awake();
        Health = 100;
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true;
        agent.updateRotation = true;
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        foreach (Transform point in mainWayPoint.transform)
        {
            patrolPoints.Add(point.position);
        }
        patrol = true;

        agent.stoppingDistance = stopDistance;

        StartCoroutine("FindTarget", 0.1f);

        fireStart = GameObject.FindGameObjectWithTag("GunT").transform;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        hitParticle = Resources.Load<GameObject>("Preffabs/Flare");
    }
    void Update()
    {
        if (visibleTargets.Count > 0) 
        {
            patrol = false;
            target = visibleTargets[0];
            aggression = true;
            if (Vector3.Distance(Position, target.position) > maxRadius)
            {
                timeOut += Time.deltaTime;
                if (timeOut > timeToWait)
                {
                    timeOut = 0;
                    visibleTargets.Clear();
                    patrol = true;
                    aggression = false;
                }
            }
        }
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
                    patrol = false;
                    findPlayer = true;
                }
            }
            if (aggression)
            {
                agent.stoppingDistance = fireDistance;
                agent.SetDestination(target.position);
                Vector3 pos = VectorUp();
                Ray ray = new Ray(pos, transform.forward);
                RaycastHit hit;
                Debug.DrawRay(ray.origin, ray.direction, Color.red);
                transform.LookAt(new Vector3(target.position.x, 0f, target.position.z));
                if (Physics.Raycast(ray, out hit, 400f, targetMask))
                {
                    if (hit.collider.tag == "Player" && !shoot)
                    {
                        agent.ResetPath();
                        Anim.SetTrigger("Shoot");
                        StartCoroutine(Shoot(hit));
                        shoot = true;
                    }
                    else
                    {
                        agent.stoppingDistance = persuingDistance;
                        agent.SetDestination(target.position);
                    }
                }
                else
                {
                    if (target)
                    {
                        agent.SetDestination(target.position);
                    }
                }
            }
            if (findPlayer)
            {
                agent.stoppingDistance = fireDistance;
                agent.SetDestination(playerTransform.position);
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

    void FindVisibleTargets()
    {
        Collider[] targetInRange = Physics.OverlapSphere(Position, maxRadius, targetMask);
        for (int i = 0; i < targetInRange.Length; i++)
        {
            Transform target = targetInRange[i].transform;
            Vector3 dirToTarget = (target.position - Position).normalized;
            float targetAngle = Vector3.Angle(transform.forward, dirToTarget);

            if ((-maxAngle) < targetAngle && targetAngle < maxAngle)
            {
                float distToTarget = Vector3.Distance(Position, target.position);
                if (!Physics.Raycast((VectorUp()), dirToTarget, obstacleMask))
                {
                    if (!visibleTargets.Contains(target))
                    {
                        visibleTargets.Add(target);
                    }
                }
            }
        }
    }

    IEnumerator Shoot(RaycastHit targetHit)
    {
        yield return new WaitForSeconds(0.5f);
        muzzleFlash.Play();
        targetHit.collider.GetComponent<ISetDmg>().SetDmg(dmg);
        GameObject temp = Instantiate(hitParticle, targetHit.point, Quaternion.identity);
        temp.transform.parent = targetHit.transform;
        Destroy(temp, 0.2f);
        shoot = false;
        Anim.SetTrigger("Shoot");
    }

    IEnumerator FindTarget(float deley)
    {
        while (true)
        {
            yield return new WaitForSeconds(deley);
            FindVisibleTargets();
        }
    }

    Vector3 VectorUp()
    {
        return transform.position + Vector3.up;
    }


}
