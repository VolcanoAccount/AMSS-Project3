using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏状态枚举
/// </summary>
public enum GameState
{
    //默认
    None,
    GameGuid,
    GamePrepare,
    Gaming,
    GameOver
}

/// <summary>
/// 游戏有限状态机控制器
/// 控制游戏状态切换
/// </summary>
public class GameStateController
{
    private static GameStateController instance;
    public static GameStateController Instance
    {
        get
        {
            instance ??= new GameStateController();
            return instance;
        }
    }

    //当前状态
    public GameState CurrentState { get; private set; }

    //先前状态
    public GameState PreState { get; private set; }

    //当前状态对象
    StateBase CurrentObj;

    //存放全部状态对象-对象池
    Dictionary<GameState, StateBase> stateDic = new Dictionary<GameState, StateBase>();

    public void OnUpdate()
    {
        CurrentObj?.OnUpdate();
    }

    /// <summary>
    /// 切换游戏状态
    /// </summary>
    /// <param name="newState">新的游戏状态</param>
    /// <param name="reCurrState">是否需要刷新状态</param>
    /// <typeparam name="K">继承自StateBase的泛型状态类</typeparam>
    public void ChangeState<K>(GameState newState, bool reCurrState = false)
        where K : StateBase, new()
    {
        //如果新状态和当前状态一致，不需要刷新状态
        if (newState.Equals(CurrentState) && !reCurrState)
            return;

        //如果当前状态对象存在，应该执行其退出
        CurrentObj?.OnExit();
        PreState = CurrentState;
        CurrentState = newState;
        //基于新状态 获得一个新的状态对象
        CurrentObj = GetStateObj<K>(newState);
        CurrentObj.OnEnter();
        Debug.Log("当前游戏状态：" + CurrentState);
        Debug.Log("先前游戏状态：" + PreState);
    }

    /// <summary>
    /// 获取状态对象
    /// </summary>
    /// <param name="stateType">GameState状态枚举</param>
    /// <typeparam name="K">继承自StateBase的泛型状态类</typeparam>
    /// <returns>返回继承自StateBase的游戏状态对象</returns>
    private StateBase GetStateObj<K>(GameState stateType)
        where K : StateBase, new()
    {
        //查看字典有没有该状态
        if (stateDic.ContainsKey(stateType))
            return stateDic[stateType];

        //字典没有该状态，实例化一个并返回
        StateBase state = new K();
        state.Init(this, stateType);
        stateDic.Add(stateType, state);
        return state;
    }
}
