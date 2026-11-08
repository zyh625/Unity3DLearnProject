using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPerson : MonoBehaviour
{
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
    private void OnEnable()
    {
        yaw = player.eulerAngles.y;
        pitch = beginPitch;
    }
    private void LateUpdate()
    {
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
        Quaternion rot = Quaternion.Euler(pitch, yaw, 0);
        Vector3 pivot = player.position + 
            new Vector3(0f, height, 0f);
        transform.position = pivot - rot * Vector3.forward * distance;
        transform.rotation = rot;
    }
}
