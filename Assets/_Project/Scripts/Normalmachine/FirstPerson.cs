using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPerson : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform eye;
    InputAction turnAction;

    [Header("Turn")]
    public float sensitivity = 0.1f;

    private float pitch;
    private int curFrame = 2;
    public bool stop = false;
    void Awake() => turnAction = InputSystem.actions.FindAction("Look");
    void Start() => Cursor.lockState = CursorLockMode.Locked;
    void OnEnable()
    {
        Vector3 e = transform.eulerAngles;          // 相机此刻的朝向（第三人称留下的）
        player.rotation = Quaternion.Euler(0f, e.y, 0f);   // 1. 角色转到相机的 yaw
        pitch = Mathf.DeltaAngle(0f, e.x);          // 2. 沿用俯仰角度，并把 350 换算成 -10
    }
    void LateUpdate()
    {
        if (stop) return;
        Vector2 turn = turnAction.ReadValue<Vector2>();
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
        player.Rotate(sensitivity * turn.x *
            Vector3.up, Space.World);
        transform.position = eye.position;
        pitch -= turn.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -80f, 80f);
        transform.localRotation
            = player.rotation * Quaternion.Euler(pitch, 0, 0);

    }
}
