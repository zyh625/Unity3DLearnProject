using UnityEngine;

public class PlayerKeys : MonoBehaviour
{
    public event System.Action<int> KeysChanged;
    int count = 0;
    public void GetKey() => KeysChanged?.Invoke(++count);
}
