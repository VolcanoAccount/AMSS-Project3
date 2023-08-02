using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AMSS;

/// <summary>
/// 游戏开始准备状态
/// </summary>
public class GamePrepare : StateBase
{
    #region 状态类周期函数
    public override void OnEnter()
    {
        //初始化Kinect
        if (gameManager.kinectControllerGO == null)
        {
            gameManager.InitKinect();
        }

        gameManager.PushPanel(UIPanelType.GamePrepare);
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
