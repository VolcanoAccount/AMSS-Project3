using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using AMSS;
using UnityEngine;
using UnityEngine.UI;

public class ClothesOptionsPanel : BasePanel
{
    public int clothIdex = 0;
    AssetBundle clothesAB;
    Image body;
    Image hair;
    Image tire;
    Button confirmBtn;
    Button forwardBtn;
    Button backwardBtn;
    Button returnBtn;

    ClothesInfoJson clothesInfoJson;
    ClothesPosInfoJson clothesPosInfoJson;

    Dictionary<int, int> clothesPosInfoDic = new Dictionary<int, int>();
    List<Clothes> cloths = new List<Clothes>();


    #region 面板周期函数
    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        clothIdex = 0;

        if (clothesAB == null) clothesAB = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + "cloth");

        if (body == null) body = transform.Find("Clothes/Body").GetComponent<Image>();
        if (hair == null) hair = transform.Find("Clothes/Hair").GetComponent<Image>();
        if (tire == null) tire = transform.Find("Clothes/Tire").GetComponent<Image>();

        if (clothesInfoJson == null && clothesPosInfoJson == null)
        {
            ParseClothesInfoJson();
        }
        InitClothList();
        GenerationClothing(clothIdex);
    }

    void Start()
    {
        confirmBtn = transform.Find("ConfirmBtn").GetComponent<Button>();
        forwardBtn = transform.Find("ForwardBtn").GetComponent<Button>();
        backwardBtn = transform.Find("BackwardBtn").GetComponent<Button>();
        returnBtn = transform.Find("ReturnBtn").GetComponent<Button>();
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        returnBtn.onClick.AddListener(OnClickReturnBtn);
        forwardBtn.onClick.AddListener(() => { SwitchClothes(1); });
        backwardBtn.onClick.AddListener(() => { SwitchClothes(0); });
    }
    void Update()
    {
        if (PlayerGestureListener.Instance.IsSwipeRight())
        {
            SwitchClothes(1);
        }
        if (PlayerGestureListener.Instance.IsSwipeLeft())
        {
            SwitchClothes(-1);
        }
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
    #endregion



    //解析服装配置表
    void ParseClothesInfoJson()
    {
        clothesInfoJson = JsonParser<ClothesInfoJson>.Parse("JsonInfo/ClothesInfoJson");
        clothesPosInfoJson = JsonParser<ClothesPosInfoJson>.Parse("JsonInfo/ClothesPositionJson");
        foreach (var item in clothesPosInfoJson.ClothesPosinfoList)
        {
            clothesPosInfoDic.Add(item.AssetName, item.Type);
        }
    }

    public void InitClothList()
    {
        cloths.Clear();
        Debug.Log("添加服装前链表的大小：" + cloths.Count);

        foreach (var item in clothesInfoJson.ClothesInfoList)
        {
            if (item.Sex == (int)PlayerManager.Instance.sex)
            {
                Clothes cloth = new Clothes();
                cloth.sex = item.Sex;
                cloth.name = item.Name;
                foreach (int assetName in item.Suit)
                {
                    int type;
                    clothesPosInfoDic.TryGetValue(assetName, out type);
                    switch (type)
                    {
                        case 1:
                            cloth.hairAsset = assetName;
                            break;
                        case 2:
                            cloth.tireAsset = assetName;
                            break;
                        case 3:
                            cloth.bodyAsset = assetName;
                            break;
                        default:
                            break;
                    }
                }
                cloths.Add(cloth);
            }
        }
        Debug.Log("添加服装后链表的大小：" + cloths.Count);
    }
    public void GenerationClothing(int index)
    {
        tire.gameObject.SetActive(false);
        hair.gameObject.SetActive(false);
        body.gameObject.SetActive(false);
        if (index >= cloths.Count)
        {
            Debug.LogError("服装下标越界，请传入正确的下标！");
        }
        else
        {
            if (cloths[index].bodyAsset != 0)
            {
                body.gameObject.SetActive(true);
                body.sprite = clothesAB.LoadAsset<Sprite>(cloths[index].bodyAsset.ToString());
            }
            if (cloths[index].hairAsset != 0)
            {
                hair.gameObject.SetActive(true);
                hair.sprite = clothesAB.LoadAsset<Sprite>(cloths[index].hairAsset.ToString());
            }
            if (cloths[index].tireAsset != 0)
            {
                tire.gameObject.SetActive(true);
                tire.sprite = clothesAB.LoadAsset<Sprite>(cloths[index].tireAsset.ToString());
            }
        }
        Debug.Log("当前服装是：" + cloths[index].name);
    }

    public void SwitchClothes(int judge)
    {
        if (judge > 0)
        {
            clothIdex++;
            if (clothIdex >= cloths.Count)
            {
                clothIdex = 0;
            }
        }
        else
        {
            clothIdex--;
            if (clothIdex < 0)
            {
                clothIdex = cloths.Count - 1;
            }
        }

        GenerationClothing(clothIdex);
    }

    void OnClickConfirmBtn()
    {
        PlayerManager.Instance.SetCloth(cloths[clothIdex]);
        UIManager.Instance.PushPanel(UIPanelType.Gaming);
    }

    void OnClickReturnBtn()
    {
        UIManager.Instance.PopPanel();
    }
}

public class Clothes
{
    public string name;
    public int sex;
    public int bodyAsset;
    public int hairAsset;
    public int tireAsset;
}