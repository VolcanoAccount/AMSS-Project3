using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePreparePanel : BasePanel
{
    Button testBtn;
    void Start()
    {
        testBtn=transform.Find("StartButton").GetComponent<Button>();
        testBtn.onClick.AddListener(OnClickStartBtn);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
    }

    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    void OnClickStartBtn()
    {
        GameStateController.Instance.ChangeState<Gaming>(GameState.Gaming);
    }
}
