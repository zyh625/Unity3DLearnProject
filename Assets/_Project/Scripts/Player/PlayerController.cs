using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerMovement PM;
    private IActorIntent input;
    private IPlayerAction[] actions;
    private IPlayerAction curAction;
    private void Awake()
    {
        PM = GetComponent<PlayerMovement>();
        actions = GetComponents<IPlayerAction>();
        input = GetComponent<IActorIntent>();
        System.Array.Sort(actions, (a, b) =>
            b.Priority.CompareTo(a.Priority));
    }
    private void Update()
    {
        if (curAction != null && !curAction.Processing) curAction = null;
        foreach(var a in actions)
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
        if (curAction != null)
        {
            //curAction.Tick();
            if (!curAction.Processing) curAction = null;
        }
        Vector2 move = curAction != null && curAction.LockMove
            ? Vector2.zero : input.Move;
        PM.Move(move, input.Run);
    }
}
