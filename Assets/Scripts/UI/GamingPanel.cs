using System.Collections;
using System.Collections.Generic;
using AMSS;
using UnityEngine;
using UnityEngine.UI;

public class GamingPanel : BasePanel
{
    SetFaceTexture setFaceTexture;
    Button returnBtn;
    Button photoBtn;
    RawImage userFaceTexRawIg;
    GameObject clothesGO;
    public Transform[] countdown = new Transform[3];

    #region  面板周期函数
    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        AssetBundleManager.Instance.LoadAssetBundle("clothesitem");

        InitButton();
        GenerationClothing();
    }

    void Start()
    {
        photoBtn = transform.Find("PhotoBtn").GetComponent<Button>();
        photoBtn.onClick.AddListener(OnClickPhotoBtn);
        for (int i = 0; i < photoBtn.transform.childCount; i++)
        {
            // 获取子物体的Transform
            Transform childTransform = photoBtn.transform.GetChild(i);
            countdown[i] = childTransform;
        }
    }

    void Update()
    {
        if (userFaceTexRawIg != null)
            userFaceTexRawIg.texture = setFaceTexture.GetFaceTex();
    }

    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnResume()
    {
        GenerationClothing();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public override void OnExit()
    {
        Destroy(clothesGO);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
    #endregion

    void GenerationClothing()
    {
        Clothes clothes = PlayerManager.Instance.chooseClothes;
        clothesGO = GameObject.Instantiate(
            AssetBundleManager.Instance.LoadAsset<GameObject>(
                "clothesitem",
                clothes.assetName.ToString()
            ),
            transform
        );
        clothesGO.transform.SetAsFirstSibling();
        if (userFaceTexRawIg == null)
            userFaceTexRawIg = clothesGO.transform
                .Find("Mask/UserFaceTex")
                .GetComponent<RawImage>();
        if (setFaceTexture == null)
            setFaceTexture = KinectManager.Instance.transform.GetComponent<SetFaceTexture>();
    }

    void OnClickReturnBtn()
    {
        UIManager.Instance.PopPanel();
    }

    void OnClickPhotoBtn()
    {
        returnBtn.gameObject.SetActive(false);
        PohtoShooter.CountdownAndMakePhoto(countdown, this, Camera.main, OnPhotoTakenCallback);
    }

    /// <summary>
    ///  图片拍摄完成后的回调函数
    /// </summary>
    /// <param name="photoByte">图片的二进制字节数组</param>
    void OnPhotoTakenCallback(byte[] photoByte)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.LoadImage(photoByte);
        PlayerManager.Instance.SetPohto(texture);
        UIManager.Instance.PushPanel(UIPanelType.Photo);
        returnBtn.gameObject.SetActive(true);
        photoBtn.gameObject.SetActive(true);
        InteractionManager.Instance.guiHandCursor.gameObject.SetActive(true);
    }

    void InitButton()
    {
        if (returnBtn != null)
        {
            Destroy(returnBtn.gameObject);
        }
        if (PlayerManager.Instance.sex == Sex.Male)
        {
            returnBtn = GameObject
                .Instantiate(Resources.Load<GameObject>("Prefabs/UI/MaleReturnBtn"), transform)
                .GetComponent<Button>();
        }
        else
        {
            returnBtn = GameObject
                .Instantiate(Resources.Load<GameObject>("Prefabs/UI/FemaleReturnBtn"), transform)
                .GetComponent<Button>();
        }
        returnBtn.onClick.AddListener(OnClickReturnBtn);
    }
}
