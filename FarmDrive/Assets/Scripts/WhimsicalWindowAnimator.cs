using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class WhimsicalWindowAnimator : MonoBehaviour
{
    [SerializeField] private float animationDuration = 0.5f; 
    [SerializeField] private Vector3 enabledScale = Vector3.one; 
    [SerializeField] private Vector3 disabledScale = Vector3.zero; 
    [SerializeField] private CanvasGroup canvasGroup; 

    private void OnEnable()
    {
        AnimateOpen();
    }

    private void OnDisable()
    {
       // AnimateClose();
    }

    private void AnimateOpen()
    {
 
        transform.localScale = disabledScale;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0;
        }
        Sequence openSequence = DOTween.Sequence();
        openSequence.Append(transform.DOScale(enabledScale, animationDuration).SetEase(Ease.OutBack));

        if (canvasGroup != null)
        {
            openSequence.Join(canvasGroup.DOFade(1, animationDuration).SetEase(Ease.Linear));
        }
    }

    public void AnimateClose()
    {
        Sequence closeSequence = DOTween.Sequence();
        closeSequence.Append(transform.DOScale(disabledScale, animationDuration).SetEase(Ease.InBack));

        if (canvasGroup != null)
        {
            closeSequence.Join(canvasGroup.DOFade(0, animationDuration).SetEase(Ease.Linear));
        }
        closeSequence.OnComplete(() => gameObject.SetActive(false));
    }
}

