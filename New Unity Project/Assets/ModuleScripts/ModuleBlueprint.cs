using UnityEngine;
using System.Collections;

public class ModuleBlueprint : MonoBehaviour {

    private int moduleID = -1;
    private bool IDset = false;

	// Use this for initialization
	void Start () {
	    if(!IDset)
        {
            moduleID = -1;
            Debug.LogError("moduleID not set for " + this.gameObject);
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
