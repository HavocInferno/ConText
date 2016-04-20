using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class TextModule : ModuleBlueprint {

    [TextArea(5, 15)]
    public string txtContent;

    private Text UIContent;

    public override GameObject getUIObject()
    {
        return UIObject;
    }

    public override void setContent(GameObject UIObjectInstance)
    {
        if (UIContent == null)
        {
            UIContent = UIObjectInstance.GetComponentInChildren<Text>();
            if (UIContent == null)
            {
                Debug.LogError("this module's UI Object is missing a Text element; " + UIObject.ToString());
            }
        }
        UIContent.text = txtContent;
    }

    public override ModuleBlueprint getNextPart()
    {
        if (subID < 4)
        {
            TextModule r = ScriptableObject.CreateInstance<TextModule>();
            r.txtContent = "this is another part";
            r.delayBeforeSend = (r.txtContent.Length * 0.2f) + 1f; //200c/m = 1/200 m/c = 60/200 s/c -> 60/200 * len = type speed (subject to change); +1 sec for send latency?
            //Debug.Log("Delay " + r.delayBeforeSend + " for " + r.txtContent.Length + " chars in text: " + r.txtContent);
            r.SetModuleID(moduleID);
            r.subID = subID + 1;
            r.previousModule = previousModule;
            r.nextModule = nextModule;
            r.UIObject = getUIObject();
            return r;
        } else { return nextModule; }
    }
}
