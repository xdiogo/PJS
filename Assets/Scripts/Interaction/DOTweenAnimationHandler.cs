using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Certifique-se de importar o namespace do DOTween

public class DOTweenAnimationHandler : MonoBehaviour
{
    [Header("Animation Settings")]
    public float scaleMultiplier = 1.5f; // Quanto o objeto vai aumentar de tamanho
    
    public float animationDuration = 1.5f; // Dura��o da rota��o/desaparo
    public float moveDistance = 5f; // Dist�ncia para o objeto sair da tela
    public Vector3 moveDirection = Vector3.up; // Dire��o para onde o objeto ser� movido

    private Transform screwMesh; // Refer�ncia para a mesh do parafuso
    private Vector3 originalScale;
    private Vector3 originalPosition;
    public bool isScrewedOut = false;

    void Start()
    {
        // Pega a mesh do parafuso (o filho)
        screwMesh = transform.Find("ScrewMesh");
        if (screwMesh == null)
        {
            Debug.LogError("ScrewMesh n�o encontrado! Certifique-se de que o objeto filho seja chamado 'ScrewMesh'.");
            return;
        }

        // Armazena os estados iniciais
        originalScale = screwMesh.localScale;
        originalPosition = screwMesh.localPosition; // Posi��o local da mesh
    }

    public void PlayAnimation()
    {
        if (isScrewedOut) return; // Evita m�ltiplas execu��es
        isScrewedOut = true;

        // Rotaciona e move a mesh para fora
        screwMesh.DOLocalRotate(new Vector3(360, 0, 0), animationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);

        screwMesh.DOLocalMove(originalPosition + moveDirection.normalized * moveDistance, animationDuration)
            .SetEase(Ease.InOutCubic);

        //screwMesh.DOScale(originalScale * scaleMultiplier, animationDuration);
    }

    public void ReturnToOriginalPosition()
    {
        if (!isScrewedOut) return; // Apenas se estiver desaparafusado
        isScrewedOut = false;

        // Interrompe anima��es em andamento
        screwMesh.DOKill();

        // Retorna a mesh � posi��o e rota��o originais
        screwMesh.DOLocalMove(originalPosition, animationDuration)
            .SetEase(Ease.InOutCubic);

        screwMesh.DOLocalRotate(Vector3.zero, animationDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutCubic);

        //screwMesh.DOScale(originalScale, animationDuration);
    }
}
