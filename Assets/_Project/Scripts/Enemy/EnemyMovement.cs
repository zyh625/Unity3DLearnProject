using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private int curPatrol = 0;
    private Transform[] patrolPlaces;
    [SerializeField] private float extraDistance = 0.5f;
    private Quaternion spawnRotation;
    private Rigidbody rb;
    private float height;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        height = GetComponent<CapsuleCollider>().bounds.size.y;
        spawnRotation = transform.rotation;
        spawnRotation.y = Random.Range(0, 360f);
        rb = GetComponent<Rigidbody>();
    }
    private void OnEnable()
    {
        transform.rotation = spawnRotation;
        agent.enabled = true;
        Vector3 born = patrolPlaces[Random.Range(0, patrolPlaces.Length)].position;
        born.y += height * 2f;
        transform.position = born;
        rb.isKinematic = true;
    }
    public void SetPatrol(Transform[] trans) => patrolPlaces = trans;
    public void Stop(bool stop) => agent.isStopped = stop;
    public bool HasArrived => Vector3.Distance(transform.position,
        patrolPlaces[curPatrol].position) <= agent.stoppingDistance + extraDistance;
    public bool PathReady => !agent.pathPending && agent.pathStatus ==
            NavMeshPathStatus.PathComplete;
    public void ChangePatrol()
    {
        curPatrol = Random.Range(0, patrolPlaces.Length);
        agent.SetDestination(patrolPlaces[curPatrol].position);
    }
    public void MoveTo(Vector3 pos) => agent.SetDestination(pos);
    public void CloseAgent() => agent.enabled = false;
    public float PlanarSpeed
    {
        get
        {
            if (!agent.isActiveAndEnabled || !agent.isOnNavMesh)
                return 0f;

            Vector3 velocity = agent.velocity;
            return new Vector2(velocity.x, velocity.z).magnitude;
        }
    }

}
