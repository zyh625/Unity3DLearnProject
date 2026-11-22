using UnityEngine;

public class PlayerCombat : MonoBehaviour,IPlayerAction
{
    [SerializeField] private BoxCollider box;
    [SerializeField] private int priority;
    public int Priority => priority;
    public bool Processing { get; private set; }

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
    public bool LockMove => Processing;
    public bool TryStart(IActorIntent a) => !Processing && a.AttackPressed
        && PM.OnGround;

    public void Begin()
    {
        Processing = true;
        PA.PlayLightAttack();
    }

    public void Tick()
    {
        
    }
    public void Cancel()
    {
        Processing = false;
        BoxControll(false);
    }
    private void BoxControll(bool flag)
    {
        if (!Processing && flag) return;
        box.gameObject.SetActive(flag);
    }
}
