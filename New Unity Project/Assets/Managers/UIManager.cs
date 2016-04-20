using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

    public bool addModule(ModuleBlueprint mod)
    {
        //Debug.Log(mod.GetType().ToString());
        GameObject UIModInstance = Instantiate(mod.getUIObject()) as GameObject;
        mod.setContent(UIModInstance);
        UIModInstance.transform.SetParent(TStream.UIContent.transform);
        UIModInstance.name = mod.GetType().ToString() + " " + mod.moduleID + " " + mod.subID;
        UIWrap.scrollToZero(); //this seems to take effect before the scrollview adjusts its height...why?
        Unify.Instance.ModMng.addModuleToDict(mod.moduleID, UIModInstance);

        return false;
    }

    public void loadUILayer(StateManager.GameState state)
    {
        switch(state)
        {
            case StateManager.GameState.MENU:
                UIWrap.ToLayer(UIWrap.MenuLayer);
                break;
            case StateManager.GameState.TEXT:
                UIWrap.ToLayer(UIWrap.TextLayer);
                break;
            case StateManager.GameState.LOG:
                UIWrap.ToLayer(UIWrap.LogLayer);
                break;
        }
    }
}
