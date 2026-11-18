using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour,IActorIntent
{
    InputAction moveAction, jumpAction, runAction;
    InputAction attackAction;
    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        runAction = InputSystem.actions.FindAction("Sprint");
        attackAction = InputSystem.actions.FindAction("Attack");
    }
    public Vector2 Move => moveAction.ReadValue<Vector2>();
    public bool Run => runAction.IsPressed();
    public bool JumpPressed => jumpAction.WasPressedThisFrame();
    public bool AttackPressed => attackAction.WasPressedThisFrame();
}
