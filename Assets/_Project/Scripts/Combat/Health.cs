using UnityEngine;
using System;

public class Health : MonoBehaviour,IDamageable
{
    [SerializeField] private float beginBlood;
    public event Action Damaged;
    public event Action Died;
    private float curBlood;

    private void OnEnable()
    {
        curBlood = beginBlood;
    }
    public void TakeDamage(DamageInfo info)
    {
        if (curBlood <= 0f) return;
        curBlood -= info.attack;
        Debug.Log(curBlood);
        if (curBlood <= 0f)
        {
            Died?.Invoke();
            return;
        }
        Damaged?.Invoke();
    }

}
