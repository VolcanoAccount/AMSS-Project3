using UnityEngine;
using System.Collections.Generic;
using AMSS;
using System;

// [Serializable]
// public class ClothesPosInfo
// {
//     public int ID;
//     public int Type;
//     public int AssetName;
// }

public class Test : MonoBehaviour
{
    public Transform[] countdown;
    void Start()
    {
        // ClothesPosInfoJson wrapper =JsonParser<ClothesPosInfoJson>.Parse("JsonInfo/ClothesPositionJson");
        // Debug.Log(wrapper.ClothesPosinfoList);
        // 遍历数组并输出信息
        PohtoShooter.CountdownAndMakePhoto(countdown,this,Camera.main);
    }
}
