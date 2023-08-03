using System;
using System.Collections.Generic;
using UnityEngine;
using AMSS;
using System.Linq;

/// <summary>
/// UI管理类
/// </summary>
public class UIManager
{
    private static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    private Dictionary<UIPanelType, string> panelPathDict = new Dictionary<UIPanelType, string>(); //存储所有面板的路径
    private Dictionary<UIPanelType, BasePanel> panelDict = new Dictionary<UIPanelType, BasePanel>(); //存储已经实例化的面板
    private Stack<BasePanel> panelStack = new Stack<BasePanel>(); //存储要操作的界面
    public Stack<BasePanel> PanelStack
    {
        get { return panelStack; }
    }

    //UI画布
    private Transform sceneCanvasTF;
    public Transform SceneCanvasTF
    {
        get
        {
            if (sceneCanvasTF == null)
            {
                sceneCanvasTF = GameObject
                    .Instantiate(Resources.Load<GameObject>("Prefabs/UI/SceneCanvas"))
                    .transform;
                sceneCanvasTF.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
                sceneCanvasTF.GetComponent<Canvas>().worldCamera = Camera.main;
            }
            return sceneCanvasTF;
        }
    }

    UIManager()
    {
        ParseUIPanelTypeJson();
    }

    //解析Json文件
    void ParseUIPanelTypeJson()
    {
        UIPanelTypeJosn jsonObject = JsonParser<UIPanelTypeJosn>.Parse("JsonInfo/UIPanelType");
        foreach (var item in jsonObject.infoList)
        {
            //存储面板路径
            panelPathDict.Add(item.uIPanelType, item.path);
        }
    }

    //实例化面板
    public BasePanel GetPanel(UIPanelType uIPanelType)
    {
        BasePanel basePanel;
        panelDict.TryGetValue(uIPanelType, out basePanel); //根据Panel类型获取对应的面板
        if (basePanel == null)
        {
            string path;
            panelPathDict.TryGetValue(uIPanelType, out path);
            if (path == null)
            {
                Debug.LogError("没有对应的路径，请查看配置表" + uIPanelType);
                return null;
            }
            else
            {
                GameObject instPanel = GameObject.Instantiate(Resources.Load<GameObject>(path));
                //面板实例化后挂载对应面板脚本
                AddScriptsComponent(uIPanelType, instPanel);
                instPanel.AddComponent<CanvasGroup>();
                instPanel.transform.SetParent(SceneCanvasTF, false);
                panelDict.Add(uIPanelType, instPanel.GetComponent<BasePanel>());
                return instPanel.GetComponent<BasePanel>();
            }
        }
        else
        {
            return basePanel;
        }
    }

    /// <summary>
    /// 给UIPanel挂载脚本
    /// </summary>
    /// <param name="uIPanelType">UIPanel类型枚举</param>
    /// <param name="instPanel">需要挂载脚本的面板</param>
    void AddScriptsComponent(UIPanelType uIPanelType, GameObject instPanel)
    {
        string str = System.Enum.GetName(typeof(UIPanelType), uIPanelType); //枚举转字符串
        string scriptsName = str + "Panel"; //获取脚本名字
        Type scripts = Type.GetType(scriptsName); //反射获取对应类型
        if (!instPanel.GetComponent(scripts))
        {
            instPanel.AddComponent(scripts);
        }
    }

    /// <summary>
    /// 挂载脚本的泛型方法
    /// </summary>
    /// <param name="instPanel">需要挂载脚本的面板</param>
    /// <typeparam name="T">面板脚本组件</typeparam>
    /// <returns>返回脚本组件</returns>
    public T GetAndAddComponent<T>(GameObject instPanel)
        where T : BasePanel
    {
        if (!instPanel.GetComponent<T>())
        {
            instPanel.AddComponent<T>();
        }
        return instPanel.GetComponent<T>();
    }

    /// <summary>
    /// 打开UIPanel
    /// </summary>
    /// <param name="uIPanelType">panel枚举类型</param>
    /// /// <returns>返回面板实例脚本</returns>
    public BasePanel PushPanel(UIPanelType uIPanelType)
    {
        if (panelStack.Count > 0)
        {
            panelStack.Peek().OnExit();
        }
        BasePanel panel = GetPanel(uIPanelType);
        panelStack.Push(panel);
        panel.OnEnter();
        Debug.Log("UI面板栈中数据长度" + panelStack.Count);
        return panel;
    }

    /// <summary>
    /// 关闭UIPanel
    /// </summary>
    public void PopPanel()
    {
        if (panelStack.Count <= 0)
        {
            return;
        }
        BasePanel Panel = panelStack.Pop();
        Panel.OnExit();
        if (panelStack.Count <= 0)
        {
            return;
        }
        panelStack.Peek().OnResume();
    }

    /// <summary>
    /// 根据面板脚本获取面板枚举类型
    /// </summary>
    /// <param name="panel">面板脚本</param>
    /// <returns>返回面板枚举类型</returns>
    public UIPanelType GetPanelType(BasePanel panel)
    {
        return panelDict.FirstOrDefault(x => x.Value.Equals(panel)).Key;
    }

    /// <summary>
    /// 清空面板实例
    /// </summary>
    /// <returns>返回是否清空</returns>
    public bool ClearAllPanel()
    {
        foreach (var item in PanelStack)
        {
            UnityEngine.Object.Destroy(item.gameObject);
        }
        PanelStack.Clear();
        panelDict.Clear();
        return true;
    }
}
