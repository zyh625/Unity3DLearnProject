using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(DamageInfo damage);
}
public class DamageInfo
{
    public Transform tran;
    public float attack;
    public DamageInfo(Transform t,float a)
    {
        tran = t;attack = a;
    }
}
