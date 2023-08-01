using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonHoverEffect : MonoBehaviour
{
    public Image targetImage;
    public Image CursorImage;
    bool isHovering;
    public float hoverScaleAmount = 1.25f;
    public float hoverDuration = 0.2f;
    private Vector3 originalScale;

    private void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        originalScale = targetImage.transform.localScale;
        CursorImage = GameObject.FindWithTag("KinectCursor").GetComponent<Image>();
    }

    void Update()
    {
        Vector2 screenPoint;
        screenPoint = RectTransformUtility.WorldToScreenPoint(
            Camera.main,
            CursorImage.transform.position
        );
        isHovering = RectTransformUtility.RectangleContainsScreenPoint(
            targetImage.GetComponent<RectTransform>(),
            screenPoint,
            Camera.main
        );
        OnPointerEnter();
        OnPointerExit();
    }

    #region KinectCursor鼠标悬停监听
    public void OnPointerEnter()
    {
        if (isHovering)
        {
            // 鼠标悬停时按钮2放大效果
            targetImage.transform
                .DOScale(originalScale * hoverScaleAmount, hoverDuration)
                .SetEase(Ease.OutQuad);
        }
    }

    public void OnPointerExit()
    {
        if (!isHovering)
        {
            // 鼠标移出时按钮2恢复正常大小
            targetImage.transform.DOScale(originalScale, hoverDuration).SetEase(Ease.OutQuad);
        }
    }
    #endregion
}
