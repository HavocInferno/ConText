using UnityEngine;
using System.Collections;

public class ModuleBlueprint : ScriptableObject {

    [TextArea(1,1)]
    public int moduleID = -1;

    public ModuleBlueprint previousModule, nextModule;

    public GameObject UIObject;

    private bool IDset = false;

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

    public virtual GameObject getUIObject()
    {
        return UIObject;
    }

    public virtual void setContent(GameObject UIObjectInstance) { }
}
