using System;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public Transform moveReference;
    public bool faceMoveDirection = true;
    public float turnSpeed = 720f;
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    CharacterController cc;
    public Vector3 Velocity => cc.velocity;
    public bool OnGround => cc.isGrounded;
    public float AirTime => airTime;
    float airTime = 0f;
    float verticalVelocity;
    private bool landThisFrame = false;
    public bool LandThisFrame => landThisFrame;
    [Header("Jump")]
    public float jumpHeight = 1.5f;
    public float gravity = -20f;
    public event Action Land;
    void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
    public void Move(Vector2 input, bool isRunning)
    {
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
        moveDir *= isRunning ? runSpeed : moveSpeed;
        bool ground = cc.isGrounded;
        if (ground)
            verticalVelocity = verticalVelocity < 0f ? -2f : verticalVelocity;
        verticalVelocity += gravity * Time.deltaTime;
        moveDir.y = verticalVelocity;
        cc.Move(moveDir * Time.deltaTime);
        bool curGround = cc.isGrounded;
        if (landThisFrame = !ground && curGround)
            Land?.Invoke();
        airTime = curGround ? 0f : airTime + Time.deltaTime;
    }
    public void Jump()=>verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
}
