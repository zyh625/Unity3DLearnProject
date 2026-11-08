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
    void Awake()
    {
        viewAction = InputSystem.actions.FindAction("SwitchView");
        first = _camera.GetComponent<FirstPerson>();
        third = _camera.GetComponent<ThirdPerson>();
        player = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        viewMode = ViewMode.FirstPerson;
        Apply(viewMode);
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
    void Update()
    {
        if (viewAction.WasPressedThisFrame())
        {
            viewMode = viewMode == ViewMode.FirstPerson ?
                ViewMode.ThirdPerson : ViewMode.FirstPerson;
            Apply(viewMode);
        }
    }
}
