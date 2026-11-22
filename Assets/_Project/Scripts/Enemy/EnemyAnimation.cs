using UnityEngine;
using System;
public class EnemyAnimation : MonoBehaviour
{
    private Animator animator;
    public event Action Death;
    public event Action damageOver;
    private EnemyMovement enemyMovement;
    public event Action<bool> punchBegin, punchEnd;
    public event Action punchOver;

    private void Awake()
    {
        enemyMovement = GetComponentInParent<EnemyMovement>();
        animator = GetComponent<Animator>();
    }
    private void LateUpdate()
    {
        animator.SetFloat("Speed", enemyMovement.PlanarSpeed);
    }
    public void Punch() => animator.SetTrigger("Attack");
    public void Damaged() => animator.SetTrigger("Damaged");
    public void Died() => animator.SetTrigger("Died");
    private void AttackBoxOpen() => punchBegin?.Invoke(true);
    private void AttackBoxClose() => punchEnd?.Invoke(false);
    private void Dead() => Death?.Invoke();
    private void DamageOver() => damageOver?.Invoke();
    private void AttackEnd() => punchOver?.Invoke();
}
