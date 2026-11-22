using System;
using UnityEngine;
using UnityEngine.AI;
public enum EnemyState { Patrol, Idle, Chase, Return, 
    Warning, Attacked, Died,Punch };
public class EnemyAI : MonoBehaviour
{

    private EnemyVision enemyVision;
    private Health health;
    private EnemyState state = EnemyState.Idle;
    private float curTime = 0f;
    private EnemyVisionLight vision;
    EnemyState priState;

    private bool Dead = false;

    public event Action EnemyDead;
    private EnemyMovement enemyMovement;
    private float curWarning = 0f;
    private float idleTime = 0f;
    [SerializeField] private float idleWait = 3.5f;
    [SerializeField] private float warningTime = 1f;
    private EnemyAnimation enemyAnimation;
    private EnemyCombat combat;

    private void Awake()
    {
        combat = GetComponent<EnemyCombat>();
        enemyVision ??= GetComponent<EnemyVision>();
        enemyMovement ??= GetComponent<EnemyMovement>();
        health = GetComponent<Health>();
        vision = GetComponentInChildren<EnemyVisionLight>();
        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
    }
    private void OnEnable()
    {
        state = EnemyState.Idle;
        priState = EnemyState.Idle;
        curTime = 0f;
        idleTime = 0f;
        curWarning = 0f;
        Dead = false;
        health.Damaged += Damaged;
        health.Died += Died;
        enemyAnimation.Death += Die;
        enemyAnimation.damageOver += AttackedOver;
        enemyAnimation.punchBegin += PunchBoxSet;
        enemyAnimation.punchEnd += PunchBoxSet;
        enemyAnimation.punchOver += AttackedOver;
    }
    private void OnDisable()
    {
        health.Damaged -= Damaged;
        health.Died -= Died;
        enemyAnimation.Death -= Die;
        enemyAnimation.damageOver -= AttackedOver;
        enemyAnimation.punchBegin -= PunchBoxSet;
        enemyAnimation.punchEnd -= PunchBoxSet;
        enemyAnimation.punchOver -= AttackedOver;
    }
    private void PunchBoxSet(bool flag) => combat.PunchBoxSet(flag);
    private void Died()
    {
        SetState(EnemyState.Died);
        enemyMovement.CloseAgent();
        Dead = true;
        enemyAnimation.Died();
    }
    public void Init(Transform[] trans, Transform p)
    {
        enemyVision = GetComponent<EnemyVision>();
        enemyMovement = GetComponent<EnemyMovement>();
        enemyMovement.SetPatrol(trans);
        enemyVision.SetPlayer(p);
    }
    private void Damaged()
    {
        if (Dead) return;
        SetState(EnemyState.Attacked);
        enemyAnimation.Damaged();
    }
    private void Update()
    {
        curTime += Time.deltaTime;
        switch (state)
        {
            case EnemyState.Idle: Idle(); break;
            case EnemyState.Chase: Chase(); break;
            case EnemyState.Patrol: Patrol(); break;
            case EnemyState.Return: Patrol(); break;
            case EnemyState.Warning: Warning(); break;
        }
    }
    private void AttackedOver() => SetState(EnemyState.Idle);
    private void Die()
    {
        EnemyDead?.Invoke();
        gameObject.SetActive(false);
    }
    private void Warning()
    {
        if (!enemyVision.SeePlayer())
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
        if (!enemyMovement.PathReady) return;
        if (enemyMovement.HasArrived)
            SetState(EnemyState.Idle);
    }
    private void Idle()
    {
        idleTime += Time.deltaTime;
        if (CheckPlayer()) return;
        if (idleTime >= idleWait) SetState(EnemyState.Patrol);
    }
    private void Chase()
    {
        if (curTime < 0.2f) return;
        curTime = 0f;
        if (!enemyVision.SeePlayer())
        {
            SetState(EnemyState.Return);
            return;
        }
        Vector3 pos = enemyVision.PlayerPlace;
        if (combat.CanAttack(pos)) 
        {
            combat.SetAttackTime();
            enemyAnimation.Punch();
            SetState(EnemyState.Punch);
            return;
        }
        enemyMovement.MoveTo(pos);
    }
    private bool CheckPlayer()
    {
        if (curTime < 0.2f) return false;
        curTime = 0f;
        if (enemyVision.SeePlayer())
        {
            enemyMovement.Stop(true);//看到玩家先停下检测是否到达阈值
            SetState(EnemyState.Warning);
            return true;
        }
        return false;
    }
    private void SetState(EnemyState nextState)
    {
        priState = state;
        enemyMovement.Stop(nextState == EnemyState.Attacked || nextState == EnemyState.Died
            || nextState == EnemyState.Idle || nextState == EnemyState.Warning
            || nextState == EnemyState.Punch);
        if (nextState == EnemyState.Patrol || nextState == EnemyState.Return)
            enemyMovement.ChangePatrol();
        else if (nextState == EnemyState.Chase)
            enemyMovement.MoveTo(enemyVision.PlayerPlace);
        state = nextState;
        if (!enemyVision.SeePlayer())
        {
            vision.UpdateColor(0f);
            curWarning = 0f;
        }
        curTime = 0f;
        idleTime = 0f;
    }
}
