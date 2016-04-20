using UnityEngine;
using System.Collections;

public class ModuleBlueprint : ScriptableObject {

    /*public class nextPart
    {
        public float delay = 0.0f;
        public string text = "";
        public bool lastOfModule = true;
        public ModuleBlueprint nextModule;
    }*/

    [TextArea(1,1)]
    public int moduleID = -1;
    public int subID = 0;

    public float delayBeforeSend = 1.0f;

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

    public virtual ModuleBlueprint getNextPart()
    {
        /*nextPart r = new nextPart();
        r.lastOfModule = true;
        r.nextModule = nextModule;
        return r;*/

        return nextModule;
    }
}
