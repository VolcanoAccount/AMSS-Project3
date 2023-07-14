using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable] // 标记为可序列化类
public class ClothInfo:ObjectInfoBase
{
    public int id;
    public string name;
    public int assetname;
    public int sexual;
    public bool visable;
}
public class ClothesInfoJson
{
    public List<ClothInfo> ClothesInfoList;
}