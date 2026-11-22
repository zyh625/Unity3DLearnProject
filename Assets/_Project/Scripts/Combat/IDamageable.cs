using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(DamageInfo damage);
}
public struct DamageInfo
{
    public Vector3 dir;
    public float attack;
    public DamageInfo(Vector3 t,float a)
    {
        dir = t;attack = a;
    }
}
