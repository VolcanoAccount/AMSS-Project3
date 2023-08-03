using UnityEngine.UI;
using AMSS;


public class SexSelectionPanel : BasePanel
{
    Button maleBtn;
    Button femaleBtn;

    #region 面板周期函数
    public override void OnEnter()
    {
        base.OnEnter();
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    void Start()
    {
        maleBtn = transform.Find("MaleBtn").GetComponent<Button>();
        maleBtn.onClick.AddListener(() =>
        {
            OnClickGenderBtn(Sex.Male);
        });
        femaleBtn = transform.Find("FemaleBtn").GetComponent<Button>();
        femaleBtn.onClick.AddListener(() =>
        {
            OnClickGenderBtn(Sex.Female);
        });
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

    //性别选择按钮监听事件
    void OnClickGenderBtn(Sex sex)
    {
        GameManager.PlayerManager.SetSex(sex);
        GameManager.PushPanel(UIPanelType.ClothesOptions);
    }
}
