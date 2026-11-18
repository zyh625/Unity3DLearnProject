using System;
using UnityEngine;

public class PlayerJump : MonoBehaviour,IPlayerAction
{
    private int curJumpTime = 0;
    [SerializeField] private int maxJumpTime = 2;
    [SerializeField] private int priority;
    public int Priority => priority;
    public bool Processing => false;
    private PlayerAnimation PA;
    public bool LockMove => false;
    private PlayerMovement PM;
    void Awake()
    {
        PM = GetComponent<PlayerMovement>();
        PA = GetComponentInChildren<PlayerAnimation>();
    }
    private void OnEnable()
    {
        PM.Land += ResetJump;
    }
    private void OnDisable()
    {
        PM.Land -= ResetJump;
    }
    public bool TryStart(IActorIntent a) => curJumpTime < maxJumpTime &&
        (PM.OnGround || curJumpTime > 0) && a.JumpPressed;
    public void Begin()
    {
        PA.PlayJump();
        PM.Jump();
        curJumpTime++;
    }

    public void Cancel()
    {
        
    }

    public void Tick()
    {
        
    }
    public void ResetJump() => curJumpTime = 0;
}
