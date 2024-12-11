using DG.Tweening;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class DOTweenAnimationHandler : MonoBehaviour
{
    [Header("Animation Settings")]
    public float screwAnimationDuration = 1.5f;
    public float wallplateAnimationDuration = 1.5f;
    public float screwMoveDistance = 0.2f;
    public float wallplateMoveDistance = 0.3f;
    public Vector3 wallplateMoveDirection = Vector3.back;

    [Header("Cinemachine Settings")]
    public CinemachineCamera targetCamera; // C�mera para transi��o

    private Transform screwMesh;
    private Vector3 screwOriginalPosition;
    private Vector3 wallplateOriginalPosition;

    public bool isScrewRemoved = false;
    public bool isWallplateRemoved = false;

    void Start()
    {
        screwMesh = transform.Find("Screw/ScrewMesh");
        if (screwMesh == null)
        {
            Debug.LogError("ScrewMesh n�o encontrado! Certifique-se de que a hierarquia est� correta.");
            return;
        }

        screwOriginalPosition = screwMesh.localPosition;
        wallplateOriginalPosition = transform.localPosition;
    }

    // Anima��o do parafuso
    public void PlayScrewAnimation()
    {
        if (isScrewRemoved) return;
        isScrewRemoved = true;

        screwMesh.DOLocalMove(screwOriginalPosition + Vector3.left * screwMoveDistance, screwAnimationDuration)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                Debug.Log("Parafuso removido.");
            });
    }

    // Anima��o da wallplate com fade e troca de c�mera
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
                StartCoroutine(SwitchCameraWithFade());
            });
    }

    private IEnumerator SwitchCameraWithFade()
    {
        var interactionHandler = FindObjectOfType<InteractionHandler>();
        if (interactionHandler != null) interactionHandler.StartTransition();

        // Fade Out
        PostProcessManager.current.FadeOut();
        yield return new WaitForSeconds(1.1f);

        // Troca para a c�mera alvo tempor�ria
        CameraManager.current.FocusOnCamera(targetCamera);

        // Fade In

        Debug.Log("FAde out");
        PostProcessManager.current.FadeOut();
        yield return new WaitForSeconds(1.1f);

        //// Pausa antes de retornar � interactionCamera (opcional)
        //yield return new WaitForSeconds(10f);

        // Fade Out para voltar � c�mera de intera��o

        // Voltar para a c�mera de intera��o

        // Fade In
        Debug.Log("FAde in");
        PostProcessManager.current.FadeIn();
        yield return new WaitForSeconds(1.1f);

        if (interactionHandler != null) interactionHandler.EndTransition();
    }
    void OnEnable()
    {
        CableConnector.OnAllCablesConnected += HandleAllCablesConnected;
    }

    void OnDisable()
    {
        CableConnector.OnAllCablesConnected -= HandleAllCablesConnected;
    }

    private void HandleAllCablesConnected()
    {
        StartCoroutine(ExecuteFadeTransition());
    }

    private IEnumerator ExecuteFadeTransition()
    {
        // Exemplo: Fade Out, troca de c�mera e Fade In
        Debug.Log("Iniciando transi��o de fade ap�s todos os cabos estarem conectados.");

        // Fade Out
        PostProcessManager.current.FadeOut();
        yield return new WaitForSeconds(1.1f);

        // Aqui voc� pode adicionar l�gica de troca de c�mera, se necess�rio
        CameraManager.current.UnfocusCamera(targetCamera);


        // Fade In
        PostProcessManager.current.FadeIn();
        yield return new WaitForSeconds(1.1f);

        Debug.Log("Transi��o de fade conclu�da.");
    }
}