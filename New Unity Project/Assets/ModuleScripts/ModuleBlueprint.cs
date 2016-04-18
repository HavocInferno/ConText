using UnityEngine;
using System.Collections;

public class ModuleBlueprint : ScriptableObject {

    [TextArea(1,1)]
    public int moduleID = -1;

    public ModuleBlueprint previousModule, nextModule;

    private bool IDset = false;

	// Use this for initialization
	public void Start () {
	    if(!IDset)
        {
            moduleID = -1;
            Debug.LogError("moduleID not set for " + this.ToString());
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public int GetModuleID ()
    { return moduleID; }

    public bool SetModuleID(int id)
    {
        if (!IDset)
        {
            moduleID = id;
            IDset = true;
        } return IDset;
    }
}
