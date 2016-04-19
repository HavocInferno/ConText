using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModuleManager : MonoBehaviour {

    public Dictionary<int, GameObject> modules = new Dictionary<int, GameObject>();
    public ModuleBlueprint firstModule;
    private ModuleBlueprint nextModule;

    // Use this for initialization
	void Start () {
        nextModule = firstModule;
        //Unify.Instance.UIMng.addModule(firstModule);
        StartCoroutine(fireNext());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addModuleToDict(int modID, GameObject inst)
    {
        modules.Add(modID, inst);
    }

    IEnumerator fireNext()
    {
        while (nextModule != null)
        {
            if (Unify.Instance.StateMng.GetGameState() == StateManager.GameState.TEXT)
            {
                yield return new WaitForSeconds(1.0f);
                Unify.Instance.UIMng.addModule(nextModule);
                nextModule = nextModule.nextModule;
            } else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
    /*in UI Manager, functions for adding and hiding messages
    in here, function for loading module data and initiating it to be added to UI (through UI Mng)
    and execute whatever is to be executed for the current module
    what about ones with replies or outcome? module itself should send reply to Module Manager, which then acts upon it.*/
}
