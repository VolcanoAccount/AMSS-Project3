using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartPanel : BasePanel
{

    void Start()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
        UIManager.Instance.SetPanelAsFirstSibling(transform);
    }

    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
}
