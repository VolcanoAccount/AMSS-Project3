using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AMSS;
using DG.Tweening;

public class GamePreparePanel : BasePanel
{
    Button startBtn;

    void Start()
    {
        startBtn = transform.Find("StartButton").GetComponent<Button>();
        startBtn.onClick.AddListener(OnClickStartBtn);
        startBtn.transform
            .DOScale(1 * 1.2f, 1f)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public override void OnExit()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }

    void OnClickStartBtn()
    {
        GameManager.Instance.isGaming = true;
        GameStateController.Instance.ChangeState<Gaming>(GameState.Gaming);
    }
}
