using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaming : StateBase
{
    public override void OnEnter()
    {
        UIManager.Instance.PushPanel(UIPanelType.SexSelection);
    }

    public override void OnExit()
    {
        
    }

    public override void OnUpdate()
    {
        
    }
}
