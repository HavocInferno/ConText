using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    public enum GameState
    {
        MENU,
        STREAM,
        LOG,
        MINIGAME
    }

    private GameState gameState = GameState.MENU;

	// Use this for initialization
	void Start () {
        Unify.Instance.UIMng.loadUILayer(gameState);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public GameState GetGameState()
    { return gameState; }

    public void SetGameState(GameState gs)
    {
        gameState = gs;
        Unify.Instance.UIMng.loadUILayer(gameState);
    }
}
