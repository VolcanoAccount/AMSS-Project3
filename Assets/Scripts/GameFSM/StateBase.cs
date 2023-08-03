using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AMSS;

/// <summary>
/// 状态基类
/// 所有状态都继承这个基类
/// </summary>
public abstract class StateBase
{
    //当前状态对象代表的枚举状态
    public GameState StateType;
    protected GameStateController gameController;
    protected GameManager GameManager;

    //首次实例化时的初始化
    public void Init(GameStateController controller, GameState stateType)
    {
        this.StateType = stateType;
        gameController = controller;
        GameManager = GameManager.Instance;
    }

    #region 状态类周期函数
    //进入
    public abstract void OnEnter();

    //更新
    public abstract void OnUpdate();

    //退出
    public abstract void OnExit();
    #endregion
}
