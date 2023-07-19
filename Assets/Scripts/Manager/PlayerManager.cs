using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Sex
{
    None,
    Male,
    Female
}

public class PlayerManager
{
    //TODO:玩家数据管理类完善
    //玩家性别
    public Sex sex { get; private set; }

    //玩家选择的服装
    public Clothes chooseClothes{get;private set;}

    private static PlayerManager instance;
    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerManager();
            }
            return instance;
        }
    }

    public void SetSex(Sex sex)
    {
        this.sex = sex;
        Debug.Log("玩家是" + sex);
    }

    public void SetCloth(Clothes clothes)
    {
        chooseClothes=clothes;
        Debug.Log("玩家选择的服装是:"+clothes.name);
    }
}