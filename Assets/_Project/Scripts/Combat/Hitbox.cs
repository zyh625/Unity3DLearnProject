using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private HashSet<IDamageable> haveAttack = new();
    [SerializeField] private float attack;
    private void OnEnable()
    {
        haveAttack.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageable enemy = other.GetComponentInParent<IDamageable>();
        if (enemy!=null&& haveAttack.Add(enemy))
        {
            enemy.TakeDamage(new(transform.position, attack));
        }
    }
}
