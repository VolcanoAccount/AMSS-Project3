using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGuid : StateBase
{
    GameObject GuidVideoGO;
    public override void OnEnter()
    {
        if(GuidVideoGO==null)
        {
            GuidVideoGO=GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/GameObject/GuidVideo"));
        }
    }

    public override void OnExit()
    {
        GuidVideoGO.SetActive(false);
    }

    public override void OnUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            gameController.ChangeState<GamePrepare>(GameState.GamePrepare);
        }
    }
}
