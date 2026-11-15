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

    private Animator animator;

    CharacterController cc;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction runAction;
    float verticalVelocity;
    public bool stop = false;
    float airTime = 0f;
    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
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
        {
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("JumpStarted");
        }
        verticalVelocity += gravity * Time.deltaTime;
        bool isRunning = runAction.IsPressed();
        moveDir *= isRunning ? runSpeed : moveSpeed;
        moveDir.y = verticalVelocity;
        cc.Move(moveDir*Time.deltaTime);
        animator.SetBool("Grounded", cc.isGrounded);
        airTime = cc.isGrounded ? 0f : airTime + Time.deltaTime;
        animator.SetFloat("AirTime", airTime);
        if (animator != null)
        {
            Vector3 velocity = cc.velocity;
            velocity.y = 0;
            animator.SetFloat("Speed", velocity.magnitude);
        }
    }
}
