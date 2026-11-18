using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private HashSet<IDamageable> haveAttack = new();
    [SerializeField] private float attack;
    private DamageInfo info;
    private void Awake()
    {
        info = new(transform, attack);
    }
    private void OnEnable()
    {
        haveAttack.Clear();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out IDamageable enemy))
        {
            if (haveAttack.Add(enemy))
            {
                enemy.TakeDamage(info);
            }
        }
    }
}
