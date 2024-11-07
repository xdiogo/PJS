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

    [Header("Camera")]
    public CinemachineCamera interactionCamera;
    public bool isInteracting;

    [Header("GameObject")]
    public GameObject player;

    // Update is called once per frame
    void Update()
    {
        CheckVision();

        CheckExitPress();
        CheckPress();

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
        interactionCamera.Priority = 15;
        interactionCamera.Follow = currentInteractor.transform;
        interactionCamera.LookAt = currentInteractor.transform;
        
        if(player != null)
        {
            player.SetActive(false);
        }


        //interactionCamera.GetCinemachineComponent<CinemachineFollow>()
    }


    void CheckExitPress()
    {
        if (!Input.GetKeyDown(KeyCode.Q)) return;

        if (currentInteractor == null) return;

        if (!isInteracting) return;

        interactionCamera.Priority = -1;

        isInteracting = false;

        if (player != null)
        {
            player.SetActive(true);
        }
    }
}
