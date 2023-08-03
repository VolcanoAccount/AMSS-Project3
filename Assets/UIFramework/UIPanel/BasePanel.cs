using UnityEngine;
using AMSS;

/// <summary>
///面板基类
/// </summary>
public class BasePanel : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    protected GameManager GameManager;

    #region 周期函数
    public virtual void OnEnter()
    {
        if (canvasGroup == null)
        {
            canvasGroup = transform.GetComponent<CanvasGroup>();
        }
        if (GameManager == null)
        {
            GameManager=GameManager.Instance;
        }
    }

    public virtual void OnPause() { }

    public virtual void OnResume() { }

    public virtual void OnExit() { }
    #endregion
}
