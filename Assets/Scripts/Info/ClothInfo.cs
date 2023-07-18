using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClothesInfo
{
    public int ID;
    public string Name;
    public int Sex;
    public int[] Suit;
    public bool Visable;
}
[Serializable]
public class ClothesInfoJson
{
    public List<ClothesInfo> ClothesInfoList;
}