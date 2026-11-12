using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public Transform moveReference;
    public bool faceMoveDirection = true;
    public float turnSpeed = 720f;
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    [Header("Jump")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;


    CharacterController cc;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction runAction;
    float verticalVelocity;
    public bool stop = false;

    void Awake()
    {
        cc = GetComponent<CharacterController>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        runAction = InputSystem.actions.FindAction("Sprint");
    }
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 moveDir = Vector3.zero;
        Vector3 v1 = moveReference.forward, v2 = moveReference.right;
        v1.y = v2.y = 0f;
        moveDir = v1.normalized * input.y +
            v2.normalized * input.x;
        if (faceMoveDirection && moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, Quaternion.LookRotation(moveDir),
               turnSpeed * Time.deltaTime);
        }
        if (cc.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
        if (cc.isGrounded && jumpAction.WasPressedThisFrame())
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        verticalVelocity += gravity * Time.deltaTime;
        bool isRunning = runAction.IsPressed();
        moveDir *= isRunning ? runSpeed : moveSpeed;
        moveDir.y = verticalVelocity;
        cc.Move(moveDir*Time.deltaTime); 
    }
}
