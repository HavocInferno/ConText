using UnityEngine;
using System.Collections;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

/*Parent class to all modules used in the framework, defines all basic functionality needed by and for interaction of any module with the various managers.*/
[System.Serializable]
public class ModuleBlueprint : ScriptableObject {

    /*moduleID and subID uniquely identify a single module (at least if not broken manually)*/
    [TextArea(1,1)]
    public int moduleID = -1;
    public int subID = 0;

    /*Delay to imitate natural latency of a message being typed as well as sent. 
    To be calculated individually inside each heir class and ideally encoded with the getNextPart return value.*/
    public float delayBeforeSend = 1.0f;

    [SerializeField]
    public ModuleBlueprint previousModule, nextModule;

    public Character sendingCharacter;
    public LogEntry log;

    //the gameobject/prefab (needs to be compatible with Unity 4.6/5.0 onward new UI) to be used as a message instance in the UI's content view.
    public GameObject UIObjectTemplate;

    //getter/setter/crude "final" workaround for defining a moduleID manually. Not used yet (Alan, please fix!)
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
        return UIObjectTemplate;
    }

    /*is given the actual instance of the prior defined UIObject and sets the corresponding info in it; 
    e.g. in TextModule this function copies its text value into the UIObjectInstance's Text property.*/
    public virtual void setContent(GameObject UIObjectInstance) { }

    /*this function is called by ModuleManager in order to get the next module (be it global or virtual) to fire. 
    If an heir of this class/module wants to await own input and send the next part whenever necessary, 
    simply return null with this.*/
    public virtual ModuleBlueprint getNextPart()
    {
        return nextModule;
    }
}
