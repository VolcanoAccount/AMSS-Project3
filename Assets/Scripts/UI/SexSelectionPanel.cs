using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class SexSelectionPanel : BasePanel
{
    Button maleBtn;
    Button femaleBtn;

    void Start()
    {
        maleBtn = transform.Find("MaleBtn").GetComponent<Button>();
        maleBtn.onClick.AddListener(OnClickMaleBtn);
        femaleBtn = transform.Find("FemaleBtn").GetComponent<Button>();
        femaleBtn.onClick.AddListener(OnClickFemaleBtn);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
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

    void OnClickMaleBtn()
    {
        PlayerManager.Instance.SetSex(Sex.Male);
        UIManager.Instance.PushPanel(UIPanelType.ClothesOptions);
    }

    void OnClickFemaleBtn()
    {
        PlayerManager.Instance.SetSex(Sex.Female);
        UIManager.Instance.PushPanel(UIPanelType.ClothesOptions);
    }
}
