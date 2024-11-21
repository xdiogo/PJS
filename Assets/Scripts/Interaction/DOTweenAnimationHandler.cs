using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; // Certifique-se de importar o namespace do DOTween

public class DOTweenAnimationHandler : MonoBehaviour
{
    [Header("Animation Settings")]
    public float scaleMultiplier = 1.5f; // Quanto o objeto vai aumentar de tamanho
    public float animationDuration = 0.5f; // Dura��o da anima��o
    public float returnDuration = 0.5f; // Dura��o para retornar ao estado original

    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale; // Armazena o tamanho original
    }

    public void PlayAnimation()
    {
        // Anima para aumentar o tamanho
        transform.DOScale(originalScale * scaleMultiplier, animationDuration)
            .OnComplete(() =>
            {
                // Ap�s terminar, volta ao tamanho original
                transform.DOScale(originalScale, returnDuration);
            });
    }
}