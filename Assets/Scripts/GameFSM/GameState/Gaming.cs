using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming : StateBase
{
    public override void OnEnter()
    {
        switch (UIManager.Instance.GetPanelType(UIManager.Instance.PanelStack.Peek()))
        {
            case UIPanelType.GamePrepare:
                UIManager.Instance.PushPanel(UIPanelType.SexSelection);
                break;
            case UIPanelType.SexSelection:
            case UIPanelType.ClothesOptions:
            case UIPanelType.Gaming:
            default:
                break;
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
}
