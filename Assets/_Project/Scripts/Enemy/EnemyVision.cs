using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    private Transform player;
    public float maxDis = 4f;
    private float maxAngle = 50;
    [SerializeField] private LayerMask layer;

    public bool SeePlayer()
    {
        if (player == null || !player.gameObject.activeInHierarchy)
            return false;
        Vector3 dir = player.position - transform.position;
        Vector3 dirFace = new(dir.x, 0, dir.z);
        Vector3 cur = transform.rotation * Vector3.forward;
        cur.y = 0;
        bool seePlayer = dir.magnitude <= maxDis &&
            !Physics.Raycast(transform.position, dir, dir.magnitude, layer)
            && Vector3.Angle(dirFace, cur) <= maxAngle;
        return seePlayer;
    }
    public void SetPlayer(Transform p) => player = p;
    public Vector3 PlayerPlace => player.position;
}
