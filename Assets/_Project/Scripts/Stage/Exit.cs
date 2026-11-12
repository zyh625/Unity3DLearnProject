using UnityEngine;

public class Exit : MonoBehaviour
{
    bool isEnd = false;
    private void OnTriggerEnter(Collider other)
    {
        if(!isEnd&&other.CompareTag("Player"))
        {
            isEnd = true;
            Debug.Log("Victory");
        }
    }
}
