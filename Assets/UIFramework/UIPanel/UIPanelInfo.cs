using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIPanelInfo:ObjectInfoBase
{
    [NonSerialized]
    public UIPanelType uIPanelType;
    public string panelTypeString;
    public string path;

    public override void OnAfterDeserialize()
    {
        uIPanelType=(UIPanelType)System.Enum.Parse(typeof(UIPanelType), panelTypeString);
    }
}

class UIPanelTypeJosn
{
    public List<UIPanelInfo> infoList;
}
