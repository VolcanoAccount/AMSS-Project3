using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using AMSS;
using UnityEngine;
using UnityEngine.UI;

public class ClothesOptionsPanel : BasePanel
{
    public int clothIdex = 0;

    GameObject btns;
    Image clothesImage;
    Button confirmBtn;
    Button forwardBtn;
    Button backwardBtn;
    Button returnBtn;

    ClothesInfoJson clothesInfoJson;
    List<Clothes> clothesList = new List<Clothes>();

    #region 面板周期函数
    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
        clothIdex = 0;

        AssetBundleManager.Instance.LoadAssetBundle("cloth");

        if (clothesInfoJson == null)
        {
            ParseClothesInfoJson();
        }
        if (clothesImage == null)
        {
            clothesImage = transform.Find("Clothes").GetComponent<Image>();
        }

        InitClothList();
        GenerationClothing(clothIdex);
        InitButton();
    }

    void Start()
    {
        // GameStateController.Instance.ChangeState<GameGuid>(GameState.GameGuid);
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
    }

    public void InitClothList()
    {
        clothesList.Clear();
        Debug.Log("添加服装前链表的大小：" + clothesList.Count);
        foreach (var item in clothesInfoJson.ClothesInfoList)
        {
            if (item.Sex == (int)GameManager.PlayerManager.Sex)
            {
                Clothes clothes = new Clothes();
                clothes.sex = item.Sex;
                clothes.name = item.Name;
                clothes.assetName = item.AssetName;
                clothesList.Add(clothes);
            }
        }
        Debug.Log("添加服装后链表的大小：" + clothesList.Count);
    }

    public void GenerationClothing(int index)
    {
        clothesImage.sprite = AssetBundleManager.Instance.LoadAsset<Sprite>(
            "cloth",
            clothesList[index].assetName.ToString()
        );
        Debug.Log("当前服装是：" + clothesList[index].name);
    }

    public void SwitchClothes(int judge)
    {
        if (judge > 0)
        {
            clothIdex++;
            if (clothIdex >= clothesList.Count)
            {
                clothIdex = 0;
            }
        }
        else
        {
            clothIdex--;
            if (clothIdex < 0)
            {
                clothIdex = clothesList.Count - 1;
            }
        }

        GenerationClothing(clothIdex);
    }

    void OnClickConfirmBtn()
    {
        GameManager.PlayerManager.SetCloth(clothesList[clothIdex]);
        GameManager.PushPanel(UIPanelType.Gaming);
    }

    void OnClickReturnBtn()
    {
        GameManager.PopPanel();
    }

    void InitButton()
    {
        if (btns != null)
        {
            Destroy(btns);
        }
        if (GameManager.PlayerManager.Sex == Sex.Male)
        {
            btns = GameObject.Instantiate(
                Resources.Load<GameObject>("Prefabs/UI/OptionsMaleButton"),
                transform
            );
            returnBtn = GameObject
                .Instantiate(Resources.Load<GameObject>("Prefabs/UI/MaleReturnBtn"), transform)
                .GetComponent<Button>();
        }
        else
        {
            btns = GameObject.Instantiate(
                Resources.Load<GameObject>("Prefabs/UI/OptionsFemaleButton"),
                transform
            );
            returnBtn = GameObject
                .Instantiate(Resources.Load<GameObject>("Prefabs/UI/FemaleReturnBtn"), transform)
                .GetComponent<Button>();
        }
        confirmBtn = btns.transform.Find("ConfirmBtn").GetComponent<Button>();
        forwardBtn = btns.transform.Find("ForwardBtn").GetComponent<Button>();
        backwardBtn = btns.transform.Find("BackwardBtn").GetComponent<Button>();
        confirmBtn.onClick.AddListener(OnClickConfirmBtn);
        returnBtn.onClick.AddListener(OnClickReturnBtn);
        forwardBtn.onClick.AddListener(() =>
        {
            SwitchClothes(1);
        });
        backwardBtn.onClick.AddListener(() =>
        {
            SwitchClothes(0);
        });
    }
}

public class Clothes
{
    public string name;
    public int sex;
    public int assetName;
}
