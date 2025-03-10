using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    public RectTransform logo;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 1;
        float screenWidth = Screen.width;
        logo.anchoredPosition = new Vector2(-screenWidth, 0);

        Sequence loadingSequence = DOTween.Sequence();

        loadingSequence.AppendInterval(2.5f);
        loadingSequence.Append(logo.DOAnchorPos(Vector2.zero, 0.8f).SetEase(Ease.OutBack));
        loadingSequence.Append(logo.DOScale(1.1f, 0.2f).SetLoops(4, LoopType.Yoyo));
        loadingSequence.Append(logo.DOAnchorPos(new Vector2(screenWidth, 0), 0.7f).SetEase(Ease.InBack));
        loadingSequence.Append(canvasGroup.DOFade(0, 0.5f));
        loadingSequence.AppendCallback(() => gameObject.SetActive(false));
    }
}