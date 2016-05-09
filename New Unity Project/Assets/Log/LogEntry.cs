using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[System.Serializable]
public class LogEntry : ScriptableObject {

    public int logID = 1;

    public LogEntry parent;
    public List<LogEntry> children = new List<LogEntry>();
    public string txtContent;

    public GameObject UIObjectTemplate;

    public GameObject getUIObject()
    {
        return UIObjectTemplate;
    }

    public void setContent(GameObject UIObjectInstance)
    {
        Debug.Log("setting content for ID " + logID);
        Text UIContent = null;
        if (UIContent == null)
        {
            UIContent = UIObjectInstance.GetComponentInChildren<ModuleUIHelper>().TextContainer.GetComponentInChildren<Text>();
            if (UIContent == null)
            {
                Debug.LogError("this log's UI Object is missing a Text element; " + getUIObject().ToString());
            }
        }

        UIContent.text = "Log @ " + System.DateTime.Now.ToString() + ": " + txtContent;
    }

    public int getParentLogID()
    {
        return parent != null ? parent.logID : -1;
    }
}
