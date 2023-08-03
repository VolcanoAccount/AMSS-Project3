using System.Collections;
using System.Collections.Generic;
using AMSS;
using UnityEngine;
using UnityEngine.UI;

public class GamingPanel : BasePanel
{
    // string bundleURL = "http://120.25.202.75:8083/StandaloneWindows/clothesitem";
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

        // if(!AssetBundleManager.Instance.LoadedAssetBundles.ContainsKey("clothesitem"))
        // {
        //     Debug.Log("clothesitem资源包未加载，现在加载该资源包！");
        //     StartCoroutine(LoadAssetBundleAndAsset());
        // }
        // else
        // {
        //     GenerationClothing();
        // }

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

    // private IEnumerator LoadAssetBundleAndAsset()
    // {
    //     yield return AssetBundleManager.Instance.LoadAssetBundleFromServer(bundleURL, "clothesitem");
    //     GenerationClothing();
    // }

    void GenerationClothing()
    {
        Clothes clothes = PlayerManager.Instance.ChooseClothes;
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
        GameManager.PopPanel();
    }

    void OnClickPhotoBtn()
    {
        returnBtn.gameObject.SetActive(false);
        photoBtn.interactable = false;
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
        GameManager.PlayerManager.SetPohto(texture);
        GameManager.PushPanel(UIPanelType.Photo);
        returnBtn.gameObject.SetActive(true);
        photoBtn.gameObject.SetActive(true);
        InteractionManager.Instance.guiHandCursor.gameObject.SetActive(true);
        photoBtn.interactable = true;
    }

    void InitButton()
    {
        if (returnBtn != null)
        {
            Destroy(returnBtn.gameObject);
        }
        if (GameManager.PlayerManager.Sex == Sex.Male)
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
