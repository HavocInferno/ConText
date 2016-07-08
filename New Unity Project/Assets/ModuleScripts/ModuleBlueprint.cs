using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

/*Parent class to all modules used in the framework, defines all basic functionality needed by and for interaction of any module with the various managers.*/
[System.Serializable]
public class ModuleBlueprint : ScriptableObject {

    /*
    IDCChoiceCapsule is a special data type to encapsulate a single ID set and be able to check whether it is equal to the ID set of a given module.
    This is used during initial loading of existing as well as saving of new story progress in order to encode the module as light as possible 
    as well as to compare the save file content with the module assets for consistency.
    */
    [System.Serializable]
    public class IDChoiceCapsule
    {
        public int c_seqID = -1;
        public int c_branchID = -1;
        public int c_hierarchyID = -1;
        public int c_subpartID = 0;

        public int choice = -1;

        public void SetModuleID(int sID, int bID, int hID, int spID)
        {
            c_seqID = sID;
            c_branchID = bID;
            c_hierarchyID = hID;
            c_subpartID = spID;
        }

        public bool checkIDequal(ModuleBlueprint mod)
        {
            Debug.Log("checking. mod: " + mod.hierarchyID + "h-" + mod.branchID + "b-" + mod.seqID + "s --- idc: " + c_hierarchyID + "h-" + c_branchID + "b-" + c_seqID + "s");

            if (mod.seqID != c_seqID)
                return false;

            if (mod.branchID != c_branchID)
                return false;

            if (mod.hierarchyID != c_hierarchyID)
                return false;

            /*if (mod.subpartID != c_subpartID)
                return false;*/

            return true;
        }

        public override string ToString()
        {
            return c_seqID + "s-" + c_branchID + "b-" + c_hierarchyID + "h-" + c_subpartID + "sp";
        }
    }

    /*moduleID and subID uniquely identify a single module (at least if not broken manually)*/
    //[TextArea(1,1)]
    public int seqID = -1;
    public int branchID = -1;
    public int hierarchyID = -1;
    public int subpartID = 0;

    /*Delay to imitate natural latency of a message being typed as well as sent. 
    To be calculated individually inside each heir class and ideally encoded with the getNextPart return value.*/
    public float delayBeforeSend = 1.0f;

    [SerializeField]
    public ModuleBlueprint previousModule, nextModule;

    public Character sendingCharacter;
    public LogEntry log;
    public AudioClip messageSound;

    //the gameobject/prefab (needs to be compatible with Unity 4.6/5.0 onward new UI) to be used as a message instance in the UI's content view.
    public GameObject UIObjectTemplate;

    //getter/setter/crude "final" workaround for defining a moduleID manually.
    private bool IDset = false;
    public int[] GetModuleID()
    {
        int[] r = { seqID, branchID, hierarchyID };
        return r;
    }
    public bool SetModuleID(int sID, int bID, int hID)
    {
            seqID = sID;
            branchID = bID;
            hierarchyID = hID;
        return true;
    }

    public virtual GameObject getUIObject()
    {
        return UIObjectTemplate;
    }

    /*is given the actual instance of the prior defined UIObject and sets the corresponding info in it; 
    e.g. in TextModule this function copies its text value into the UIObjectInstance's Text property.*/
    public virtual void setContent(GameObject UIObjectInstance)
    {
        if (sendingCharacter != null)
        {
            foreach (Image ia in UIObjectInstance.GetComponentsInChildren<Image>())
            {
                ia.color = sendingCharacter.blobColor;
            }
        }
    }

    /*this function is called by ModuleManager in order to get the next module (be it global or virtual) to fire. 
    If an heir of this class/module wants to await own input and send the next part whenever necessary, 
    simply return null with this.*/
    public virtual ModuleBlueprint getNextPart()
    {
        return nextModule;
    }

    /*This hands over the given Capsule to the Module Manager for saving in the progress save file.*/
    public virtual void pushChoice(IDChoiceCapsule idc)
    {
        if (!Unify.Instance.StateMng.initialLoad)
            Unify.Instance.ModMng.addChoiceToList(this, idc);
        else
            Debug.Log("Not pushing due to initial load");
    }

    /*This is used when loading a save file and displaying the progress up to that point. 
    Given is the choice capsule as well as the choice ID in question. 
    This function should then return the corresponding next module of this module. 
    e.g. in a Reply Module this function might return outcome #3 when given the parameter choiceID = 3.*/
    public virtual ModuleBlueprint getModForChoice(int choiceID, IDChoiceCapsule idc)
    {
        return nextModule;
    }

    /*This function should return the highest module that can be found above this module. 
    Essentially quasi recursive, it should query all next modules for their respective highest module (e.g. by calling their getHighestModule() function) 
    and then return the "highest" (i.e. with highest ID set) of the returned modules. Otherwise return itself.*/
    public virtual ModuleBlueprint getHighestModule()
    {
        if (nextModule != null)
            return nextModule.getHighestModule();
        else
            return this;
    }

    /*This function is similarly quasi recursive as getHighestModule(). 
    It is intended to fix the ID sets of the entire active story by starting with the firstModule specified in the module manager. 
    It should simply consider its own ID and given next modules are present, adapt their IDs as the logical next ID set and then call this function on these next modules.
    Given the function is implemented correctly across all module classes, the process will work and the entire story will have correct ID sets.*/
    public virtual void fixNextIDs()
    {
        if(nextModule != null)
        {
            nextModule.SetModuleID(seqID + 1, branchID, hierarchyID);
            nextModule.fixNextIDs();
        }
    }

    public virtual void resetModule() { }
}
