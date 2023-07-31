using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : StateBase
{
    public override void OnEnter()
    {
        if (gameManager.GameOver())
        {
            gameController.ChangeState<GameGuid>(GameState.GameGuid);
        }
    }

    public override void OnExit() { }

    public override void OnUpdate() { }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}
