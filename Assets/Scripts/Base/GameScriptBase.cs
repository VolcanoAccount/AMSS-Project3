using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AMSS;

public class GameScriptBase : MonoBehaviour
{
    protected GameManager gameManager;
    virtual protected void Awake()
    {
        gameManager = GameManager.instance;
    }
}
