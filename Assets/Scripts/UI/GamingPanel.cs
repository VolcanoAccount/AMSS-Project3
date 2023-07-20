using System.Collections;
using System.Collections.Generic;
using AMSS;
using UnityEngine;
using UnityEngine.UI;

public class GamingPanel : BasePanel
{
    SetFaceTexture setFaceTexture;
    Button returnBtn;
    Image body;
    Image hair;
    Image tire;
    RawImage userFaceTexRawIg;


    #region  面板周期函数
    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;


        if (body == null) body = transform.Find("Clothes/Body").GetComponent<Image>();
        if (hair == null) hair = transform.Find("Clothes/Hair").GetComponent<Image>();
        if (tire == null) tire = transform.Find("Clothes/Tire").GetComponent<Image>();
        if (userFaceTexRawIg == null) userFaceTexRawIg = transform.Find("Mask/UseFaceTex").GetComponent<RawImage>();
        if (setFaceTexture == null) setFaceTexture = KinectManager.Instance.transform.GetComponent<SetFaceTexture>();

        GenerationClothing();
    }
    void Start()
    {
        returnBtn = transform.Find("ReturnBtn").GetComponent<Button>();
        returnBtn.onClick.AddListener(OnClickReturnBtn);
    }
    void Update()
    {
        userFaceTexRawIg.texture =setFaceTexture.GetFaceTex();
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

    void GenerationClothing()
    {
        Clothes clothes = PlayerManager.Instance.chooseClothes;
        tire.gameObject.SetActive(false);
        hair.gameObject.SetActive(false);
        body.gameObject.SetActive(false);
        if (clothes != null)
        {
            if (clothes.bodyAsset != 0)
            {
                body.gameObject.SetActive(true);
                body.sprite = AssetBundleManager.Instance.LoadAsset<Sprite>("cloth", clothes.bodyAsset.ToString());
            }
            if (clothes.hairAsset != 0)
            {
                hair.gameObject.SetActive(true);
                hair.sprite = AssetBundleManager.Instance.LoadAsset<Sprite>("cloth", clothes.hairAsset.ToString());
            }
            if (clothes.tireAsset != 0)
            {
                tire.gameObject.SetActive(true);
                tire.sprite = AssetBundleManager.Instance.LoadAsset<Sprite>("cloth", clothes.tireAsset.ToString());
            }
        }
    }

    void OnClickReturnBtn()
    {
        UIManager.Instance.PopPanel();
    }
}
