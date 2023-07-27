using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : StateBase
{
    public override void OnEnter()
    {
        UIManager.Instance.PushPanel(UIPanelType.GameOver);
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }
}
