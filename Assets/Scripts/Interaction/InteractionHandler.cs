using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

using UnityEngine;

public class InteractionHandler : MonoBehaviour
{
    public Interactor currentInteractor;

    [Header("Raycast")]
    public float rayDistance = 6f;
    public LayerMask rayMask = 1;
    RaycastHit rayHit;

    [Header("Camera")]
    public CinemachineCamera interactionCamera;
    public bool isInteracting;

    [Header("GameObject")]
    public GameObject player;

    [Header("Point-and-Click")]
    public bool pointAndClickMode = false;

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

    void CheckPress()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        if (currentInteractor == null) return;

        isInteracting = true;
        pointAndClickMode = true;

        interactionCamera.Priority = 15;
        interactionCamera.Follow = currentInteractor.transform;
        interactionCamera.LookAt = currentInteractor.transform;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (player != null)
        {
            player.SetActive(false);
        }
    }

    void CheckExitPress()
    {   
        

        if (!Input.GetKeyDown(KeyCode.Q)) return;

        if (currentInteractor == null) return;

        if (!isInteracting) return;

        interactionCamera.Priority = -1;
        isInteracting = false;
        pointAndClickMode = false;

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
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            DOTweenAnimationHandler dotweenHandler = hit.collider.GetComponentInParent<DOTweenAnimationHandler>();
            if (dotweenHandler != null)
            {
                dotweenHandler.PlayAnimation();
            }
        }
    }
}

