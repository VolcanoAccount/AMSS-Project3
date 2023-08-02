using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 存储UIPanelInfoJson数据的类
/// </summary>
[Serializable]
public class UIPanelInfo:ISerializationCallbackReceiver
{
    [NonSerialized]
    public UIPanelType uIPanelType;
    public string panelTypeString;
    public string path;

    public void OnAfterDeserialize()
    {
        uIPanelType=(UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
    }

    public void OnBeforeSerialize()
    {
        
    }
}

class UIPanelTypeJosn
{
    public List<UIPanelInfo> infoList;
}
