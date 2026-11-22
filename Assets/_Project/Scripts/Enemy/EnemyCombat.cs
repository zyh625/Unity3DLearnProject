using System;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private BoxCollider punchBox;
    private float lastAttack = -1f;
    [SerializeField] private float attackWait = 3f;
    [SerializeField] private float attackDistance = 1f;
    public void SetAttackTime() => lastAttack = Time.time;
    public void PunchBoxSet(bool flag) => punchBox.gameObject.SetActive(flag);
    public bool CanAttack(Vector3 pos) {
        pos.y = 0;
        Vector3 cur = transform.position;
        cur.y = 0;
        return (lastAttack == -1f || (Time.time - lastAttack >= attackWait))
            && Vector3.Distance(pos, cur) <= attackDistance;
    }
}
