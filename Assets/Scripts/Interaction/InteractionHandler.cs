using Unity.Cinemachine;
using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public Interactor currentInteractor;

    [Header("Raycast")]
    public float rayDistance = 6f;
    public LayerMask rayMask = 1;
    RaycastHit rayHit;
    public float sphereRad = 0.1f;

    [Header("Camera")]
    public CinemachineVirtualCameraBase interactionCamera;
    public bool isInteracting;

    [Header("GameObject")]
    public GameObject player;

    [Header("Point-and-Click")]
    public bool pointAndClickMode = false;

    [Header("Cinemachine Follow Offset")]
    public CinemachineFollow cinemachineFollow;
    public CinemachineVirtualCameraBase playerCamera;
    

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
        if (isTransitioning) return; // Pausa a lógica de interação durante a transição

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

        if (!isHit)
        {
            currentInteractor = null;
            return;
        }

        bool isInteractor = rayHit.collider.TryGetComponent<Interactor>(out Interactor interactor);

        if (!isInteractor)
        {
            currentInteractor = null;
            return;
        }

        currentInteractor = interactor;
    }

    public void CheckPress()
    {
        if (currentInteractor != null) return;

        if (!Input.GetKeyDown(KeyCode.E)) return;

        CheckVision();

        if (currentInteractor == null) return;

        isInteracting = true;
        pointAndClickMode = true;

        interactionCamera.Priority = 15;
        interactionCamera.Follow = currentInteractor.transform;
        interactionCamera.LookAt = currentInteractor.transform;

        if (cinemachineFollow != null && currentInteractor != null)
        {
            cinemachineFollow.FollowOffset = currentInteractor.lookOffset;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (player != null)
        {
            player.SetActive(false);
        }
    }

    public void CheckExitPress()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;

        if (currentInteractor == null) return;

        if (!isInteracting) return;

        // Mudar a prioridade da câmera de interação para um valor baixo
        interactionCamera.Priority = -1;

        // Restaurar a prioridade da câmera do jogador (assegure-se de que a variável playerCamera está atribuída)
        if (playerCamera != null)
        {
            playerCamera.Priority = 10;  // Defina a prioridade da câmera do jogador (ajuste conforme necessário)
            playerCamera.Follow = player.transform;  // Garante que a câmera do jogador volte a seguir o player
            playerCamera.LookAt = player.transform;  // Garante que a câmera do jogador olhe para o player
        }

        // Reiniciar o estado de interação
        isInteracting = false;
        pointAndClickMode = false;

        if (cinemachineFollow != null)
        {
            cinemachineFollow.FollowOffset = Vector3.zero;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (player != null)
        {
            player.SetActive(true);
        }
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

    public void StartTransition()
    {
        isTransitioning = true;
    }

    public void EndTransition()
    {
   
        isTransitioning = false;

    }
}