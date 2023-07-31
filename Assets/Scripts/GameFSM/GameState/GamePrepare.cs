using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AMSS;

public class GamePrepare : StateBase
{
    public override void OnEnter()
    {
        if (gameManager.kinectControllerGO == null)
        {
            gameManager.InitKinect();
        }
        UIManager.Instance.PushPanel(UIPanelType.GamePrepare);
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
}
