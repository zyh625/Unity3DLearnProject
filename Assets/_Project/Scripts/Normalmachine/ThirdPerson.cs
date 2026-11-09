using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPerson : MonoBehaviour
{
    public float smoothTime = 0.2f;
    public LayerMask collisionMask;
    public float checkRadius;
    [SerializeField] private Transform player;
    [SerializeField] private float beginPitch = 20f;
    public float distance = 5f;
    public float height = 1.5f;
    public float sensitivity = 0.1f;
    public float minPitch = -30f;
    public float maxPitch = 70f;
    InputAction lookAction;
    float yaw, pitch;
    private int curFrame = 2;
    private void Awake()=>lookAction = InputSystem.actions.FindAction("Look");
    void Start() => Cursor.lockState = CursorLockMode.Locked;
    private float curDistance;
    float velocity;
    public bool stop = false;
    private void OnEnable()
    {
        yaw = player.eulerAngles.y;
        pitch = beginPitch;
        curDistance = distance;
        velocity = 0;
    }
    private void LateUpdate()
    {
        if (stop) return;
        Vector2 turn = lookAction.ReadValue<Vector2>();
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            turn = Vector2.zero;
            curFrame = 2;
        }
        else if (curFrame > 0)
        {
            turn = Vector2.zero;
            curFrame--;
        }
        yaw = Mathf.Repeat(yaw + turn.x * sensitivity, 360f);
        pitch -= turn.y * sensitivity;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);//屏幕朝向
        Vector3 pivot = player.position + 
            new Vector3(0f, height, 0f);//相机环绕中心
        Vector3 backward = (rot * -Vector3.forward);
        bool hitSomething = Physics.SphereCast(
            pivot,
            checkRadius,
            backward,
            out RaycastHit hit,
            distance,
            collisionMask
            );
        float tarDistance = hitSomething ? hit.distance : distance;
        if (tarDistance > curDistance)//平滑过渡
            curDistance = Mathf.SmoothDamp
                (curDistance, tarDistance, ref velocity, smoothTime);
        else //立即拉近
        {
            curDistance = tarDistance;
            velocity = 0;
        }
        transform.position = pivot + backward * curDistance;
        transform.rotation = rot;
    }
}
