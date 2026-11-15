using UnityEngine.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;

public class Background : MonoBehaviour
{
    private InputAction changeBG;
    private Material sky;
    private Light sun;
    private float beginLight;
    private float curLight;
    private void Awake()
    {
        sun = GetComponent<Light>();
        changeBG=InputSystem.actions.FindAction("Background");
        sky = RenderSettings.skybox;
        curLight=beginLight = sun.intensity;
    }
    bool dark = true;
    private void Update()
    {
        if (changeBG.WasPressedThisFrame())
        {
            dark = !dark;
            RenderSettings.skybox = dark ? null : sky;
            curLight = beginLight * (dark ? 1f : 3f);
            sun.intensity = curLight;
            RenderSettings.ambientMode = dark ? AmbientMode.Flat
                : AmbientMode.Skybox;
        }
    }
}
