using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class DOTweenAnimationHandler : MonoBehaviour
{
    
    public CableConnector connector;

    [Header("Animation Settings")]
    public float screwAnimationDuration = 1.5f;
    public float wallplateAnimationDuration = 1.5f;
    public float screwMoveDistance = 0.3f;
    public float wallplateMoveDistance = 0.3f;
    public Vector3 wallplateMoveDirection = Vector3.back;
    public Vector3 screwMoveDirection = Vector3.back;

    [Header("Cinemachine Settings")]
    public CinemachineCamera targetCamera;

    private Transform screwMesh;
    private Vector3 screwOriginalPosition;
    private Vector3 wallplateOriginalPosition;

    public bool isScrewRemoved = false;
    public bool isWallplateRemoved = false;

    private bool hasFaded = false;

    void Start()
    {
        screwMesh = transform.Find("Screw/ScrewMesh");
        if (screwMesh == null)
        {
            Debug.LogError("ScrewMesh não encontrado! Certifique-se de que a hierarquia está correta.");
            return;
        }

        screwOriginalPosition = screwMesh.localPosition;
        wallplateOriginalPosition = transform.localPosition;
    }

    public void PlayScrewAnimation()
    {
        if (isScrewRemoved) return;
        isScrewRemoved = true;

        screwMesh.DOLocalRotate(new Vector3(360, 0, 0), screwAnimationDuration, RotateMode.FastBeyond360)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Incremental);

        screwMesh.DOLocalMove(screwOriginalPosition + Vector3.left * screwMoveDistance, screwAnimationDuration)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                Debug.Log("Parafuso removido.");
            });
    }

    public void PlayWallplateAnimation()
    {
        if (!isScrewRemoved)
        {
            Debug.LogWarning("Remova o parafuso primeiro!");
            return;
        }

        if (isWallplateRemoved) return;
        isWallplateRemoved = true;

        transform.DOLocalMove(wallplateOriginalPosition + wallplateMoveDirection * wallplateMoveDistance, wallplateAnimationDuration)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                Debug.Log("Wallplate removida.");
                if (!hasFaded) StartCoroutine(SwitchCameraWithFade());
            });
    }

    public void ReturnToOriginalPositions()
    {
        if (isScrewRemoved)
        {
            screwMesh.DOKill();
            screwMesh.DOLocalMove(screwOriginalPosition, screwAnimationDuration).SetEase(Ease.InOutCubic);
            screwMesh.DOLocalRotate(Vector3.zero, screwAnimationDuration, RotateMode.FastBeyond360).SetEase(Ease.InOutCubic);
            isScrewRemoved = false;
        }

        if (isWallplateRemoved)
        {
            transform.DOLocalMove(wallplateOriginalPosition, wallplateAnimationDuration).SetEase(Ease.InOutCubic);
            isWallplateRemoved = false;
        }
    }

    private IEnumerator SwitchCameraWithFade()
    {
        if (hasFaded) yield break;
        hasFaded = true;
        
        var interactionHandler = FindObjectOfType<InteractionHandler>();
        if (interactionHandler != null) interactionHandler.StartTransition();

        PostProcessManager.current.FadeOut();

        connector.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.1f);


        CameraManager.current.FocusOnCamera(targetCamera);

       

        PostProcessManager.current.FadeIn();
        yield return new WaitForSeconds(1.1f);

        ReturnToOriginalPositions();
        if (interactionHandler != null) interactionHandler.EndTransition();

        hasFaded = false;
    }

    void OnEnable()
    {
        connector.OnAllCablesConnected += HandleAllCablesConnected;
        connector.OnCancel += HandleAllCablesConnected;
    }

    void OnDisable()
    {
        connector.OnAllCablesConnected -= HandleAllCablesConnected;
        connector.OnCancel -= HandleAllCablesConnected;
    }

    private void HandleAllCablesConnected()
    {
        if (!hasFaded) StartCoroutine(ExecuteFadeTransition());
    }

    private IEnumerator ExecuteFadeTransition()
    {
        Debug.Log("Iniciando transição de fade após todos os cabos estarem conectados.");
        connector.gameObject.SetActive(false);
        PostProcessManager.current.FadeOut();
        yield return new WaitForSeconds(1.1f);  

        GetComponent<SocketRepairNotifier>()?.NotifySocketRepaired();

        CameraManager.current.UnfocusCamera(targetCamera);
        Debug.Log($"objeto {gameObject.name}  camera {targetCamera.name} ");

        PostProcessManager.current.FadeIn();
        yield return new WaitForSeconds(1.1f);

        Debug.Log("Transição de fade concluída.");
    }

}