using UnityEngine;
using UnityEngine.InputSystem;

public class ViewSwitcher : MonoBehaviour
{
    public enum ViewMode { FirstPerson,ThirdPerson};
    private FirstPerson first;
    private ThirdPerson third;
    private PlayerMovement player;
    public Transform _camera;
    public ViewMode viewMode;
    InputAction viewAction;
    InputAction stopAction;
    Renderer[] bodyRenderers;
    bool stop = false;
    public float maxCameraDis = 1f;
    bool curShow = false;
    public Transform eye;
    void Awake()
    {
        bodyRenderers = GetComponentsInChildren<Renderer>();
        SetBodyRenderer();
        viewAction = InputSystem.actions.FindAction("SwitchView");
        stopAction = InputSystem.actions.FindAction("Stop");
        first = _camera.GetComponent<FirstPerson>();
        third = _camera.GetComponent<ThirdPerson>();
        player = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        viewMode = ViewMode.FirstPerson;
        Apply(viewMode);
    }
    private void SetBodyRenderer()
    {
        foreach (Renderer r in bodyRenderers)
            r.shadowCastingMode = curShow ?
                UnityEngine.Rendering.ShadowCastingMode.On :
                UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }
    private void Apply(ViewMode mode)
    {
        if (mode == ViewMode.FirstPerson)
        {
            first.enabled = true;
            third.enabled = false;
            player.faceMoveDirection = false;
            player.moveReference = player.transform;
        }
        else if (mode == ViewMode.ThirdPerson)
        {
            third.enabled = true;
            first.enabled = false;
            player.faceMoveDirection = true;
            player.moveReference = _camera;
        }
    }
    private void LateUpdate()
    {
        bool show = maxCameraDis < Vector3.Distance(
            eye.position,
            Camera.main.transform.position);
        if (show != curShow)
        {
            curShow = show;
            SetBodyRenderer();
        }
    }
    void Update()
    {
        if (stopAction.WasPressedThisFrame())
        {
            player.enabled = stop;
            Cursor.lockState = stop ? CursorLockMode.Locked : CursorLockMode.None;
            stop = !stop;
            first.stop = third.stop = stop;
        }
        if (stop) return;
        if (viewAction.WasPressedThisFrame())
        {
            viewMode = viewMode == ViewMode.FirstPerson ?
                ViewMode.ThirdPerson : ViewMode.FirstPerson;
            Apply(viewMode);
        }
    }
}
