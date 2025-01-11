using Unity.Cinemachine;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    [Header("Raycast")]
    public float rayDistance = 6f;
    public LayerMask rayMask = 1;
    RaycastHit rayHit;
    public float sphereRad = 0.1f;

    [Header("Camera")]
    public CinemachineVirtualCameraBase interactionCamera;
    public CinemachineVirtualCameraBase playerCamera;
    public CinemachineFollow cinemachineFollow;
    public bool isInteracting;

    [Header("GameObject")]
    public GameObject player;
    public GameObject Head;

    [Header("Point-and-Click")]
    public bool pointAndClickMode = false;

    private Interactor currentInteractor;
    private bool isTransitioning = false;

    void Start()
    {
        if (interactionCamera == null)
        {
            Debug.LogError("Interaction Camera não atribuída!");
        }
    }

    void Update()
    {
        if (isTransitioning) return;
        CheckVision();
        CheckPress();
        CheckExitPress();
    }

    private void FixedUpdate()
    {
        CheckMouseClick();
    }

    void CheckVision()
    {
        var mainCamera = Camera.main.transform;
        bool isHit = Physics.Raycast(mainCamera.position, mainCamera.forward, out rayHit, rayDistance, rayMask);
        Debug.DrawLine(mainCamera.position, mainCamera.position + mainCamera.forward * rayDistance, isHit ? Color.red : Color.cyan);

        if (isHit && rayHit.collider.TryGetComponent(out Interactor interactor))
        {
            currentInteractor = interactor;
            CursorManager.current.SetcursorInteraction();
        }
        else
        {
            currentInteractor = null;
            CursorManager.current.Setcursornone();
        }
    }

    public void CheckPress()
    {
        if (currentInteractor == null || !Input.GetKeyDown(KeyCode.E)) return;

        isInteracting = true;
        pointAndClickMode = true;

        interactionCamera.Priority = 15;
        interactionCamera.Follow = currentInteractor.transform;
        interactionCamera.LookAt = currentInteractor.transform;

        if (cinemachineFollow != null)
        {
            cinemachineFollow.FollowOffset = currentInteractor.lookOffset;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player?.SetActive(false);
    }

    public void CheckExitPress()
    {
        if (!Input.GetKeyDown(KeyCode.Q) || !isInteracting) return;

        interactionCamera.Priority = -1;

        if (playerCamera != null)
        {
            playerCamera.Priority = 10;
            playerCamera.Follow = Head.transform;
            playerCamera.LookAt = Head.transform;
        }

        isInteracting = false;
        pointAndClickMode = false;

        if (cinemachineFollow != null)
        {
            cinemachineFollow.FollowOffset = Vector3.zero;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player?.SetActive(true);
        currentInteractor = null;
    }

    void CheckMouseClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.SphereCast(ray, sphereRad, out RaycastHit hit, rayDistance))
        {
            var dotweenHandler = hit.collider.GetComponentInParent<DOTweenAnimationHandler>();
            if (dotweenHandler != null)
            {
                if (hit.collider.CompareTag("Screw"))
                {
                    dotweenHandler.PlayScrewAnimation();
                }
                else if (hit.collider.CompareTag("Wallplate"))
                {
                    dotweenHandler.PlayWallplateAnimation();
                }
            }
        }
    }

    public void StartTransition() => isTransitioning = true;
    public void EndTransition() => isTransitioning = false;
}
