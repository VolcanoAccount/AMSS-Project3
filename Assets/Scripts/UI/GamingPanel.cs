using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamingPanel : BasePanel
{

    Button returnBtn;


    #region  面板周期函数
    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;

        if (returnBtn == null) returnBtn=transform.Find("ReturnBtn").GetComponent<Button>();
    }
    void Start()
    {
        returnBtn.onClick.AddListener(OnClickReturnBtn);
    }
    void Update()
    {

    }
    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }
    public override void OnExit()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
    #endregion

    void OnClickReturnBtn()
    {
        UIManager.Instance.PopPanel();
    }
}
