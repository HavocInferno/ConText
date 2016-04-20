using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ModuleManager : MonoBehaviour {

    public Dictionary<Pair<int,int>, GameObject> modules = new Dictionary<Pair<int,int>, GameObject>();
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

    public void addModuleToDict(int modID, int subID, GameObject inst)
    {
        modules.Add(new Pair<int, int>(modID, subID), inst);
    }

    IEnumerator fireNext()
    {
        while (nextModule != null)
        {
            if (Unify.Instance.StateMng.GetGameState() == StateManager.GameState.TEXT)
            {
                yield return new WaitForSeconds(nextModule.delayBeforeSend);
                Unify.Instance.UIMng.addModule(nextModule);
                nextModule = nextModule.getNextPart();
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
