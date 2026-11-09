using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;
using System.Collections;

public class ViewSwitchCM : MonoBehaviour
{
    public GameObject fpCam;
    public GameObject tpCam;
    public enum ViewMode { FirstPerson, ThirdPerson };
    private PlayerMovement player;
    public Transform _camera;
    public ViewMode viewMode;
    InputAction viewAction;
    InputAction stopAction;
    Renderer[] bodyRenderers;
    bool stop = false;
    public float maxCameraDis = 1f;
    bool curShow = false;
    private CinemachineBrain CB;
    Coroutine faceRoutine;
    void Awake()
    {
        CB = Camera.main.GetComponent<CinemachineBrain>();
        bodyRenderers = GetComponentsInChildren<Renderer>();
        SetBodyRenderer();
        viewAction = InputSystem.actions.FindAction("SwitchView");
        stopAction = InputSystem.actions.FindAction("Stop");
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
            if (faceRoutine != null)
                StopCoroutine(faceRoutine);
            fpCam.SetActive(true);
            tpCam.SetActive(false);
            player.faceMoveDirection = false;
            player.moveReference = player.transform;
        }
        else if (mode == ViewMode.ThirdPerson)
        {
            player.faceMoveDirection = false;
            tpCam.SetActive(true);
            fpCam.SetActive(false);
            faceRoutine = StartCoroutine(EnableFaceAfterBlend());
            player.moveReference = _camera;
        }
    }
    private void LateUpdate()
    {
        bool show = maxCameraDis < Vector3.Distance(
            fpCam.transform.position,
            _camera.transform.position);
        if (show != curShow)
        {
            curShow = show;
            SetBodyRenderer();
        }
    }
    IEnumerator EnableFaceAfterBlend()
    {
        yield return null;
        while (CB.IsBlending)
            yield return null;
        player.faceMoveDirection = true;
    }
    void Update()
    {
        if (stopAction.WasPressedThisFrame())
        {
            player.enabled = stop;
            Cursor.lockState = stop ? CursorLockMode.Locked : CursorLockMode.None;
            if (stop)
                Apply(viewMode);
            else
            {
                fpCam.SetActive(false);
                tpCam.SetActive(false);
            }
            stop = !stop;
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
