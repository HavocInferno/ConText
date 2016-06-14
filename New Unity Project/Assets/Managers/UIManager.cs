using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class UIManager : MonoBehaviour {

    public UIWrapper UIWrap;
    public UISettings UISettings;
    public GameObject TextStreamUIObject;
    public GameObject LogStreamUIObject;

    /*instantiates the specified module's UI prefab, 
    then orders the module(asset) to set the content of the new UI instance accordingly, 
    then child-orders the instance into the text stream, 
    >ideally< scrolls down to it (not quite working yet), 
    and adds the instance to the runtime module dictionary*/
    public bool addModule(ModuleBlueprint mod)
    {
        GameObject UIModTemplate;
        Debug.Log(mod.GetType().ToString());
        if (Unify.Instance.ModMng.UITemplateMapping.TryGetValue(mod.GetType().ToString(), out UIModTemplate))
        {
            GameObject UIModInstance = Instantiate(UIModTemplate) as GameObject;
            mod.setContent(UIModInstance);
            if (UIModInstance != null)
            {
                UIModInstance.transform.SetParent(TextStreamUIObject.transform);
                UIModInstance.name = mod.GetType().ToString() + " " + mod.seqID + "s " + mod.branchID + "b " + mod.hierarchyID + "h " + mod.subpartID + "sp - instance[" + UIModInstance.GetInstanceID() + "]";
                UIWrap.scrollToZero(); //this seems to take effect before the scrollview adjusts its height...why?
                Unify.Instance.ModMng.addModuleToDict(UIModInstance.GetInstanceID()/*mod.moduleID, mod.subID*/, UIModInstance);

                return true;
            }
        }

        return false;
    }

    public bool addLogEntry(LogEntry log)
    {
        GameObject UIModInstance;
        Unify.Instance.LogMng.logEntries.TryGetValue(log.logID, out UIModInstance);
        if(UIModInstance == null) { 
            UIModInstance = Instantiate(log.getUIObject()) as GameObject;
            Unify.Instance.LogMng.addLogToDict(log.logID, UIModInstance);

            GameObject par;
            Unify.Instance.LogMng.logEntries.TryGetValue(log.getParentLogID(), out par);
            UIModInstance.transform.SetParent(par != null ? par.transform : LogStreamUIObject.transform);

            UIModInstance.name = log.GetType().ToString() + " " + log.logID;
        }
        log.setContent(UIModInstance);
        //UIWrap.scrollToZero(); //this seems to take effect before the scrollview adjusts its height...why?
        
        return true;
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
