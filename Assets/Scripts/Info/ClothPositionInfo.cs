using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClothesPosInfo
{
    public int ID;
    public int Type;
    public int AssetName;
}

[Serializable]
public class ClothesPosInfoJson
{
    public List<ClothesPosInfo> ClothesPosinfoList;
}
