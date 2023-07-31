using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button button2;
    public float hoverScaleAmount = 1.25f;
    public float hoverDuration = 0.2f;

    private Vector3 originalScale;

    private void Start()
    {
        if (button2 == null)
            button2 = GetComponent<Button>();

        // 记录按钮2的初始缩放
        originalScale = button2.transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 鼠标悬停时按钮2放大效果
        button2.transform.DOScale(originalScale * hoverScaleAmount, hoverDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 鼠标移出时按钮2恢复正常大小
        button2.transform.DOScale(originalScale, hoverDuration).SetEase(Ease.OutQuad);
    }
}
