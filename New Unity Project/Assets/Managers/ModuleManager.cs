using UnityEngine;
using System.Collections;

public class ModuleManager : MonoBehaviour {

    public IDictionary modules;
    public ModuleBlueprint firstModule;

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /*in UI Manager, functions for adding and hiding messages
    in here, function for loading module data and initiating it to be added to UI (through UI Mng)
    and execute whatever is to be executed for the current module
    what about ones with replies or outcome? module itself should send reply to Module Manager, which then acts upon it.*/
}
