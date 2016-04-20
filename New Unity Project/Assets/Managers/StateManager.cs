using UnityEngine;
using System.Collections;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class StateManager : MonoBehaviour {

    public enum GameState
    {
        MENU,
        TEXT,
        LOG,
    }

    private GameState gameState = GameState.MENU;

    public GameState GetGameState()
    { return gameState; }

    public void SetGameState(GameState gs)
    {
        gameState = gs;
    }
}
