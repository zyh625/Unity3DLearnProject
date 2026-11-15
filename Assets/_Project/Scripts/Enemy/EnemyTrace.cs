using UnityEngine;
using UnityEngine.AI;

public class EnemyTrace : MonoBehaviour
{
    private enum EnemyState { Patrol,Idle,Chase,Return,Warning};
    private EnemyState state = EnemyState.Idle;
    [SerializeField] private LayerMask layer;
    public Transform player;
    private float curTime = 0f;
    private NavMeshAgent agent;
    public float maxDis = 4f;
    private float maxAngle = 50;
    private float idleTime = 0f;
    private int curPatrol = 0;
    private EnemyVisionLight vision;
    [SerializeField] private Transform[] patrolPlaces;
    [SerializeField] private float extraDistance = 0.5f;
    [SerializeField] private float warningTime = 1f;
    private float curWarning = 0f;
    EnemyState priState;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponentInChildren<EnemyVisionLight>();
    }
    private void Update()
    {
        curTime += Time.deltaTime;
        switch (state)
        {
            case EnemyState.Idle:Idle();break;
            case EnemyState.Chase:Chase();break;
            case EnemyState.Patrol:Patrol();break;
            case EnemyState.Return:Patrol();break;
            case EnemyState.Warning:Warning();break;
        }
    }
    private void Warning()
    {
        if (!SeePlayer())
        {
            curWarning -= Time.deltaTime;
            if (curWarning <= 0f)
            {
                SetState(priState);
                return;
            }
        }
        else curWarning += Time.deltaTime;
        if (curWarning < warningTime)
            vision.UpdateColor(curWarning / warningTime);
        else
        {
            SetState(EnemyState.Chase);
            vision.UpdateColor(1f);
        }
    }
    private void Patrol()
    {
        if (CheckPlayer()) return;
        if (agent.pathPending || agent.pathStatus !=
            NavMeshPathStatus.PathComplete) return;
        if (agent.remainingDistance <= agent.stoppingDistance + extraDistance)
            SetState(EnemyState.Idle);
    }
    private void Idle()
    {
        idleTime += Time.deltaTime;
        if (CheckPlayer())return;
        if (idleTime >= 5f)SetState(EnemyState.Patrol);
    }
    private void Chase()
    {
        if (curTime < 0.2f) return;
        curTime = 0f;
        if (!SeePlayer())
        {
            SetState(EnemyState.Return);
            return;
        }
        agent.SetDestination(player.position);
    }
    private bool CheckPlayer()
    {
        if (curTime < 0.2f) return false;
        curTime = 0f;
        if (SeePlayer())
        {
            agent.isStopped = true;//看到玩家先停下检测是否到达阈值
            SetState(EnemyState.Warning);
            return true;
        }
        return false;
    }
    private bool SeePlayer()
    {
        Vector3 dir = player.position - transform.position;
        Vector3 dirFace = new(dir.x, 0, dir.z);
        Vector3 cur = transform.rotation * Vector3.forward;
        cur.y = 0;
        bool seePlayer = dir.magnitude <= maxDis &&
            !Physics.Raycast(transform.position, dir, dir.magnitude, layer)
            && Vector3.Angle(dirFace, cur) <= maxAngle;
        return seePlayer;
    }
    private void SetState(EnemyState nextState)
    {
        priState = state;
        agent.isStopped = nextState == EnemyState.Idle||nextState==EnemyState.Warning;
        if (nextState == EnemyState.Patrol || nextState == EnemyState.Return)
        {
            curPatrol = Random.Range(0, patrolPlaces.Length);
            agent.SetDestination(patrolPlaces[curPatrol].position);
        }
        else if (nextState == EnemyState.Chase)
            agent.SetDestination(player.position);
        state = nextState;
        vision.UpdateColor(0f);
        curTime = 0f;
        idleTime = 0f;
        curWarning = 0f;
    }
}
