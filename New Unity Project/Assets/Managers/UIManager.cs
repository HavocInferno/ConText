using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    public UIWrapper UIWrap;
    public UISettings UISettings;
    public TextStream TStream;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void loadUILayer(StateManager.GameState state)
    {
        switch(state)
        {

        }
    }
}
