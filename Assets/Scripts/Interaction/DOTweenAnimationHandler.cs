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
    public CinemachineVirtualCameraBase targetCamera; // C�mera para transi��o

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
        // Fade out
        PostProcessManager.current.FadeOut();
        yield return new WaitForSeconds(1f); // Sincroniza com a dura��o do fade

        // Troca para a c�mera desejada
        if (targetCamera != null)
        {
            var allCameras = FindObjectsOfType<CinemachineVirtualCameraBase>();
            foreach (var cam in allCameras)
            {
                cam.Priority = 0; // Redefine a prioridade de todas as c�meras
            }
            targetCamera.Priority = 10; // Define a prioridade da c�mera de destino
        }

        // Fade in
        PostProcessManager.current.FadeIn();
        yield return new WaitForSeconds(1f); // Sincroniza com a dura��o do fade
    }
}