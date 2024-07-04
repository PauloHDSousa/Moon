using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerActions PlayerActionsControls { get; private set; }

    private void Awake()
    {
        PlayerActionsControls = new PlayerActions();
    }

    private void OnEnable()
    {
        PlayerActionsControls.Enable();
    }

    private void OnDisable()
    {
        PlayerActionsControls.Disable();
    }
}
