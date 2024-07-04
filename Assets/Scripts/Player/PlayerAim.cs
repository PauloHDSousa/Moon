using UnityEngine;

public class PlayerAim : MonoBehaviour
{

    [Header("References")]
    [SerializeField] PlayerInput playerInput;
    [SerializeField] PlayerMovement playerMovement;

    [Header("Aim")]
    [SerializeField] LayerMask aimLayerMask;
    [SerializeField] Transform aim;

    [Space]
    [Header("Camera Info")]
    [SerializeField] Transform cameraTarget;
    [Range(.5f, 1f)]
    [SerializeField] float minCameraDistance = 1.5f;
    [Range(1f, 4f)]
    [SerializeField] float maxCameraDistance = 4f;
    [Range(3f, 5f)]
    [SerializeField] float cameraSensitivity = 5f;

    private Vector2 aimInput;
    private RaycastHit lastKnowMouseHit;

    void Start()
    {
        playerInput.PlayerActionsControls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        playerInput.PlayerActionsControls.Character.Aim.canceled += context => aimInput = Vector2.zero;
    }

    void Update()
    {
        UpdateAimPosition();
    }

    private void UpdateAimPosition()
    {
        Vector3 mousePos = GetMouseHitInfo().point;
        Vector3 aimPos = new Vector3(mousePos.x, transform.position.y + 1, mousePos.z);
        aim.position = aimPos;
        cameraTarget.position = Vector3.Lerp(cameraTarget.position, DesiredCameraPosition(), cameraSensitivity * Time.deltaTime);
    }

    public Vector3 DesiredCameraPosition()
    {

        float actualMaxCameraDistance = playerMovement.moveInput.y < -0.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMouseHitInfo().point;

        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesiredPosition = Vector3.Distance(transform.position, desiredCameraPosition);

        float clampDistance = Mathf.Clamp(distanceToDesiredPosition, minCameraDistance, actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampDistance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }

    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);
        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnowMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnowMouseHit;
    }
}
