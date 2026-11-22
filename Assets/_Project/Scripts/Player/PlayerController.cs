using UnityEngine;
using UnityEngine.InputSystem.Processors;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private PlayerReaction reaction;
    private PlayerMovement PM;
    private IActorIntent input;
    private IPlayerAction[] actions;
    private IPlayerAction curAction;
    private bool attacked = false, died = false;

    private void Awake()
    {
        playerAnimation = GetComponentInChildren<PlayerAnimation>();
        reaction = GetComponent<PlayerReaction>();
        PM = GetComponent<PlayerMovement>();
        actions = GetComponents<IPlayerAction>();
        input = GetComponent<IActorIntent>();
        System.Array.Sort(actions, (a, b) =>
            b.Priority.CompareTo(a.Priority));
    }
    private void OnEnable()
    {
        reaction.Attacked += Damaged;
        reaction.Dying += Died;
        playerAnimation.damageOver += Recover;
        playerAnimation.Death += Dead;
    }
    private void OnDisable()
    {
        reaction.Attacked -= Damaged;
        reaction.Dying -= Died;
        playerAnimation.damageOver -= Recover;
        playerAnimation.Death -= Dead;
    }

    private void Update()
    {
        if (curAction != null && !curAction.Processing) curAction = null;
        if (!died&&!attacked)
        {
            foreach (var a in actions)
            {
                if (curAction != null && a.Priority <= curAction.Priority) break;
                if (a.TryStart(input))
                {
                    curAction?.Cancel();
                    curAction = a;
                    a.Begin();
                    break;
                }
            }
        }
        if (curAction != null)
        {
            //curAction.Tick();
            if (!curAction.Processing) curAction = null;
        }
        Vector2 move = (curAction != null && curAction.LockMove)||died||attacked
            ? Vector2.zero : input.Move;
        PM.Move(move, input.Run);
    }
    private void Damaged()
    {
        if (died) return;
        attacked = true;
        ActionLock();
    }
    private void Died()
    {
        died = true;
        ActionLock();
    }
    private void ActionLock()
    {
        curAction?.Cancel();
        curAction = null;
    }
    private void Recover()
    {
        if (died) return;
        attacked = false;
    }
    private void Dead() => gameObject.SetActive(false);
}
