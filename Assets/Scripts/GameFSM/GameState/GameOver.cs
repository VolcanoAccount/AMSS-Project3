using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏结束状态
/// </summary>
public class GameOver : StateBase
{
    #region 状态类周期函数
    public override void OnEnter()
    {
        if (GameManager.GameOver())
        {
            gameController.ChangeState<GameGuid>(GameState.GameGuid);
        }
    }

    public override void OnExit() { }

    public override void OnUpdate() { }

    void Start() { }

    void Update() { }
    #endregion
}
