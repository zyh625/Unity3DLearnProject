using UnityEngine;

public class PlayerCombat : MonoBehaviour,IPlayerAction
{
    [SerializeField] private BoxCollider box;
    [SerializeField] private int priority;
    public int Priority => priority;
    public bool Processing { get; private set; }

    [SerializeField] private float attackTime;
    private PlayerAnimation PA;
    private PlayerMovement PM;
    private void Awake()
    {
        PA = GetComponentInChildren<PlayerAnimation>();
        PM = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        PA.LightEnd += Cancel;
        PA.LightBoxControll += BoxControll;
    }
    private void OnDisable()
    {
        PA.LightEnd -= Cancel;
        PA.LightBoxControll -= BoxControll;
    }
    private float lastAttackTime = -1f;
    public bool LockMove => Processing;
    public bool TryStart(IActorIntent a) => !Processing && a.AttackPressed
        && PM.OnGround && (lastAttackTime == -1f || 
        Time.time - lastAttackTime >= attackTime);

    public void Begin()
    {
        lastAttackTime = Time.time;
        Processing = true;
        PA.PlayLightAttack();
    }

    public void Tick()
    {
        
    }
    public void Cancel()
    {
        lastAttackTime = -1f;
        Processing = false;
    }
    private void BoxControll(bool flag) => box.gameObject.SetActive(flag);
}
