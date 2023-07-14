using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    //默认
    None,
    GameGuid,
    Gaming,
    Paused,
    GameOver
}


/// <summary>
/// 游戏状态机控制器
/// </summary>
public class GameStateController
{
    private static GameStateController instance;
    public static GameStateController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameStateController();
            }
            return instance;
        }
    }
    //当前状态
    private GameState currentState;
    public GameState CurrentState
    {
        get { return currentState; }
        set { currentState = value; }
    }

    //当前状态对象
    StateBase CurrentObj;

    //存放全部状态对象-对象池
    Dictionary<GameState, StateBase> stateDic = new Dictionary<GameState, StateBase>();

    void Start()
    {
        ChangeState<GameGuid>(GameState.GameGuid);
    }

    public void OnUpdate()
    {
        if (CurrentObj != null) CurrentObj.OnUpdate();
    }

    /// <summary>
    /// 修改状态
    /// </summary>
    public void ChangeState<K>(GameState newState, bool reCurrState = false) where K : StateBase, new()
    {
        //如果新状态和当前状态一致，不需要刷新状态
        if (newState.Equals(CurrentState) && !reCurrState) return;

        //如果当前状态对象存在，应该执行其退出
        if (CurrentObj != null) CurrentObj.OnExit();
        CurrentState = newState;
        //基于新状态 获得一个新的状态对象
        CurrentObj = GetStateObj<K>(newState);
        CurrentObj.OnEnter();
        Debug.Log(stateDic.Count);
    }

    /// <summary>
    /// 获取状态对象
    /// </summary>
    private StateBase GetStateObj<K>(GameState stateType) where K : StateBase, new()
    {
        //查看字典有没有该状态
        if (stateDic.ContainsKey(stateType)) return stateDic[stateType];

        //字典没有该状态，实例化一个并返回
        StateBase state = new K();
        state.Init(this, stateType);
        stateDic.Add(stateType, state);
        return state;
    }
}
