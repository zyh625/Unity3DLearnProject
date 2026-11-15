using UnityEngine;

public class EnemyVisionLight : MonoBehaviour
{
    private Color beginColor;
    private Light visionLight;
    private void Awake() => visionLight = GetComponent<Light>();
    private void Start() => beginColor = visionLight.color;
    public void UpdateColor(float awareness) => visionLight.color =
        Color.Lerp(beginColor, Color.red, awareness);
}
