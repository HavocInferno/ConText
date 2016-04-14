using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    public enum GameState
    {
        MENU,
        TEXT,
        LOG,
    }

    private GameState gameState = GameState.MENU;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameState GetGameState()
    { return gameState; }

    public void SetGameState(GameState gs)
    {
        gameState = gs;
    }
}
