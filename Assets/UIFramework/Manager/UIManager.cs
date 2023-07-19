using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AMSS;

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

    private Dictionary<UIPanelType, string> panelPathDict = new Dictionary<UIPanelType, string>();//存储所有面板的路径
    private Dictionary<UIPanelType, BasePanel> panelDict = new Dictionary<UIPanelType, BasePanel>();//存储已经实例化的面板
    private Stack<BasePanel> panelStack = new Stack<BasePanel>();//存储要操作的界面

    private Transform sceneCanvasTF;
    public Transform SceneCanvasTF
    {
        get
        {
            if (sceneCanvasTF == null)
            {
                sceneCanvasTF = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/UI/SceneCanvas")).transform;
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
            panelPathDict.Add(item.uIPanelType, item.path);
        }
    }

    //实例化面板
    private BasePanel GetPanel(UIPanelType uIPanelType)
    {
        BasePanel basePanel;
        panelDict.TryGetValue(uIPanelType, out basePanel);//根据Panel类型获取对应的面板
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
    /// 给UIPanel添加脚本
    /// </summary>
    /// <param name="uIPanelType"></param>
    /// <param name="instPanel"></param>
    void AddScriptsComponent(UIPanelType uIPanelType, GameObject instPanel)
    {
        string str = System.Enum.GetName(typeof(UIPanelType), uIPanelType);//枚举转字符串
        string scriptsName = str + "Panel";//获取脚本名字
        Type scripts = Type.GetType(scriptsName);//反射获取对应类型
        if (!instPanel.GetComponent(scripts))
        {
            instPanel.AddComponent(scripts);
        }
    }

    public T GetAndAddComponent<T>(GameObject instPanel) where T : Component
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
    public void PushPanel(UIPanelType uIPanelType)
    {
        if (panelStack.Count > 0)
        {
            panelStack.Peek().OnPause();
        }
        BasePanel panel = GetPanel(uIPanelType);
        panelStack.Push(panel);
        panel.OnEnter();
        Debug.Log("UI面板栈中数据长度" + panelStack.Count);
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
    /// Panel层级置顶
    /// </summary>
    /// <param name="tf"></param>
    public void SetPanelAsFirstSibling(Transform tf)
    {
        tf.SetAsFirstSibling();
    }
}
