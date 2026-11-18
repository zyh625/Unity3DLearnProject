using UnityEngine;

public class Health : MonoBehaviour,IDamageable
{
    [SerializeField] private float beginBlood;
    private float curBlood;

    private void Awake()
    {
        curBlood = beginBlood;
    }
    public void TakeDamage(DamageInfo info)
    {
        curBlood -= info.attack;
        Debug.Log("Attacked, curBlood equals " + curBlood);
    }
}
