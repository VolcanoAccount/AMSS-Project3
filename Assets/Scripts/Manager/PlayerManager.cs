using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sex
{
    None,
    Male,
    Female
}

/// <summary>
/// 玩家数据管理类
/// </summary>
public class PlayerManager
{
    //玩家性别
    public Sex Sex { get; private set; }

    //玩家选择的服装
    public Clothes ChooseClothes{get;private set;}

    //玩家拍的照片
    public Texture2D PohtoTex{get;private set;}

    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            instance ??= new PlayerManager();
            return instance;
        }
    }

    public void SetSex(Sex sex)
    {
        this.Sex = sex;
        Debug.Log("玩家是" + sex);
    }

    public void SetCloth(Clothes clothes)
    {
        ChooseClothes=clothes;
        Debug.Log("玩家选择的服装是:"+clothes.name);
    }

    public void SetPohto(Texture2D Tex)
    {
        PohtoTex=Tex;
    }
}