using System;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement PM;
    public event Action LightEnd;
    public event Action<bool> LightBoxControll;
    private void Awake()
    {
        PM = GetComponentInParent<PlayerMovement>();
        animator = GetComponent<Animator>();
    }
    private void LateUpdate()
    {
        if (animator != null)
        {
            animator.SetBool("Grounded", PM.OnGround);
            animator.SetFloat("AirTime", PM.AirTime);
            Vector3 velocity = PM.Velocity;
            velocity.y = 0;
            animator.SetFloat("Speed", velocity.magnitude);
        }
    }
    public void PlayJump()
    {
        animator.SetTrigger("JumpStarted");
    }

    public void PlayLightAttack()
    {
        animator.SetTrigger("Light");
    }
    private void AttackEnd() => LightEnd?.Invoke();
    private void AttackBoxOpen() => LightBoxControll?.Invoke(true);
    private void AttackBoxClose() => LightBoxControll?.Invoke(false);
}
