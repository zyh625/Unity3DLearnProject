using UnityEngine;

public class Key : MonoBehaviour
{
    public float spinSpeed;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerKeys playerKeys))
        {
            playerKeys.GetKey();
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        transform.Rotate(spinSpeed * Time.deltaTime * Vector3.up,
            Space.World);//绕竖直方向旋转
    }
}
