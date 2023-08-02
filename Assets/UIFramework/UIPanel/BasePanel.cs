using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
///面板基类
/// </summary>
public class BasePanel : MonoBehaviour
{
    protected CanvasGroup canvasGroup;

    #region 周期函数
    public virtual void OnEnter()
    {
        if (canvasGroup == null)
        {
            canvasGroup = transform.GetComponent<CanvasGroup>();
        }
    }

    public virtual void OnPause() { }

    public virtual void OnResume() { }

    public virtual void OnExit() { }
    #endregion
}
