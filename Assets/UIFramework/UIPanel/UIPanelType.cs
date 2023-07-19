using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * UIPanelType.cs
 * 枚举了所有ui面板的类型
 * UIPanelType.json
 * 保存了所有面板与其对应的路径
 * UIPanelInfo.cs
 * 与json文本一一对应的类
 * UIManager.cs
 * 1.解析保存所有面板的信息（panelPathDict）
 * 2.存储所有创建出来的面板实例（panelDict）
 * 
 * BasePanel.cs
 * 作为所有面板的基类存在的
 */
public enum UIPanelType
{
    GamePrepare,
    SexSelection,
    ClothesOptions,
    Gaming
}
