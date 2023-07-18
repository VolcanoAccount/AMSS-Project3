using System.Collections;
using System.Collections.Generic;
using AMSS;
using UnityEngine;
using UnityEngine.UI;

public class ClothOptionsPanel : BasePanel
{
    Button confirmBtn;
    Button forwardBtn;
    Button backwardBtn;
    void Start()
    {
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();
        forwardBtn = transform.Find("ForwardBtn").GetComponent<Button>();
        backwardBtn = transform.Find("BackwardBtn").GetComponent<Button>();
    }

    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
        ParseClothesInfoJson(); 
    }

    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    void InitializeClothing()
    {

    }

    //解析服装主配置表
    void ParseClothesInfoJson()
    {
        ClothesInfoJson clothesInfoJson = JsonParser<ClothesInfoJson>.Parse("JsonInfo/ClothesInfoJson");
        ClothesPosInfoJson clothesPosInfoJson =JsonParser<ClothesPosInfoJson>.Parse("JsonInfo/ClothesPositionJson");
    }
}
