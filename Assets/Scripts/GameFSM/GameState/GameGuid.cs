using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// GameGuid游戏状态类，播放引导动画
/// </summary>
public class GameGuid : StateBase
{
    GameObject GuidVideoGO;
    VideoPlayer videoPlayer;

    #region 状态类周期函数
    public override void OnEnter()
    {
        if (GuidVideoGO == null)
        {
            GuidVideoGO = GameObject.Instantiate(
                Resources.Load<GameObject>("Prefabs/GameObject/GuidVideo")
            );

            videoPlayer = GuidVideoGO.transform
                .Find("Canvas/GuidPanel")
                .GetComponent<VideoPlayer>();
        }
        GuidVideoGO.SetActive(true);
    }

    public override void OnExit()
    {
        GuidVideoGO.SetActive(false);
    }

    public override void OnUpdate()
    {
        //判断播放完毕游戏状态切换到哪个状态
        if (!videoPlayer.isPlaying && videoPlayer.time > 0f)
        {
            if (GameManager.isGaming)
            {
                gameController.ChangeState<Gaming>(GameState.Gaming);
            }
            else
            {
                gameController.ChangeState<GamePrepare>(GameState.GamePrepare);
            }
        }
        else
        {
            if (!GameManager.isGaming)
            {
                if (KinectManager.Instance.IsUserDetected())
                {
                    gameController.ChangeState<GamePrepare>(GameState.GamePrepare);
                }
            }
        }
    }
    #endregion
}
