using UnityEngine;

public class Door : MonoBehaviour
{
    public PlayerKeys playerKeys;
    public float upSpeed = 0.1f;
    bool isOpening = false;
    private Vector3 beginPos;
    private float height;
    public int keyNum;
    private void Awake()
    {
        beginPos = transform.position;
        height = GetComponent<Renderer>().bounds.size.y;
    }
    private void OnEnable()
    {
        playerKeys.KeysChanged += OnKeyChanged;
        isOpening = false;
        transform.position = beginPos;
    }
    private void OnDisable() => playerKeys.KeysChanged -= OnKeyChanged;
    public void OnKeyChanged(int count)
    {
        if (count >= keyNum) 
            isOpening = true;
    }
    private void Update()
    {
        if (isOpening)
        {
            transform.position += upSpeed * Time.deltaTime * Vector3.up;
            if (transform.position.y - beginPos.y > height)
            {
                Debug.Log("DoorOpen");
                gameObject.SetActive(false);
            }
        }
    }
}
