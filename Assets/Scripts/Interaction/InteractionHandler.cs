using System.Collections;
using System.Collections.Generic;
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
    public CinemachineCamera interactionCamera;
    public bool isInteracting;

    [Header("GameObject")]
    public GameObject player;

    [Header("Point-and-Click")]
    public bool pointAndClickMode = false;

    [Header("Cinemachine Follow Offset")]
    public CinemachineFollow cinemachineFollow;




    // Update is called once per frame
    void Update()
    {


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
        if (!Input.GetKeyDown(KeyCode.E)) return;

        if (currentInteractor == null) return;

        isInteracting = true;
        pointAndClickMode = true;

        // Atualiza a prioridade e segue o objeto de interação
        interactionCamera.Priority = 15;
        interactionCamera.Follow = currentInteractor.transform;
        interactionCamera.LookAt = currentInteractor.transform;

        // Ajusta o Follow Offset da Cinemachine
        if (cinemachineFollow != null && currentInteractor != null)
        {
            cinemachineFollow.FollowOffset = currentInteractor.lookOffset;
        }

        // Configura o cursor e desativa o jogador
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

        // Reseta a prioridade e desativa o modo de interação
        interactionCamera.Priority = -1;
        isInteracting = false;
        pointAndClickMode = false;

        // Reseta o Follow Offset da Cinemachine para um valor padrão
        if (cinemachineFollow != null)
        {
            cinemachineFollow.FollowOffset = Vector3.zero; // Altere para o valor padrão que desejar
        }

        // Configura o cursor e ativa o jogador
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
            // Detecta o objeto clicado
            var dotweenHandler = hit.collider.GetComponentInParent<DOTweenAnimationHandler>();
            if (dotweenHandler != null)
            {
                // Verifica qual objeto foi clicado
                if (hit.collider.CompareTag("Screw")) // Certifique-se de taguear o Screw como "Screw"
                {
                    dotweenHandler.PlayScrewAnimation();
                }
                else if (hit.collider.CompareTag("Wallplate")) // Certifique-se de taguear a Wallplate como "Wallplate"
                {
                    dotweenHandler.PlayWallplateAnimation();
                }
            }
        }
    }

    void ChangeToTargetCamera()
    {
        interactionCamera.Priority = 15; // Ajusta a prioridade da câmera
        interactionCamera.Follow = null; // Ajusta o alvo da câmera, se necessário
        interactionCamera.LookAt = null; // Ajusta para onde a câmera olha, se necessário
    }
}
