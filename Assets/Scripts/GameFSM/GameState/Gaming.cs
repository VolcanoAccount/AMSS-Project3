using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏中状态
/// </summary>
public class Gaming : StateBase
{
    #region 状态类周期函数
    public override void OnEnter()
    {
        if (
            gameManager.GetPanelType(gameManager.UIManager.PanelStack.Peek())
            == UIPanelType.GamePrepare
        )
        {
            gameManager.PushPanel(UIPanelType.SexSelection);
        }
    }

    public override void OnExit() { }

    public override void OnUpdate()
    {
        if (!KinectManager.Instance.IsUserDetected())
        {
            gameManager.StartTimer();
        }
        else
        {
            gameManager.StopTimer();
        }
    }
    #endregion
}
