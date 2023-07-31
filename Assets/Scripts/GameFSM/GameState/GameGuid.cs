using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class GameGuid : StateBase
{
    GameObject GuidVideoGO;
    VideoPlayer videoPlayer;

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
        //动画播放完毕
        if (!videoPlayer.isPlaying && videoPlayer.time > 0f)
        {
            if (gameManager.isGaming)
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
            if (!gameManager.isGaming)
            {
                if (KinectManager.Instance.IsUserDetected())
                {
                    gameController.ChangeState<GamePrepare>(GameState.GamePrepare);
                }
            }
        }
    }
}
