using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class TextModule : ModuleBlueprint {

    [TextArea(5, 15)]
    public string txtContent;

    private Text UIContent;
    //this has the module's text parsed
    private List<TextParser.ParsedChunk> allnextParts;

    public override GameObject getUIObject()
    {
        return UIObjectTemplate;
    }

    public override void setContent(GameObject UIObjectInstance)
    {
        Debug.Log("setting content for ID " + moduleID + ", subID " + subID);
        if (UIContent == null)
        {
            UIContent = UIObjectInstance.GetComponentInChildren<ModuleUIHelper>().TextContainer.GetComponentInChildren<Text>();
            if (UIContent == null)
            {
                Debug.LogError("this module's UI Object is missing a Text element; " + getUIObject().ToString());
            }
        }
        if (allnextParts == null || allnextParts.Count == 0 )
        {
            Debug.Log("text not parsed yet? parsing...");
            allnextParts = TextParser.parse(txtContent);
        } else
        {
            Debug.Log("text already parsed into " + allnextParts.Count + " parts.");
            foreach(TextParser.ParsedChunk a in allnextParts)
            {
                Debug.Log(a.ToString());
            }
        }

        UIContent.text = allnextParts[0].text;
        delayBeforeSend = allnextParts[0].delay;
        allnextParts.RemoveAt(0);
    }

    public override ModuleBlueprint getNextPart()
    {
        if(allnextParts.Count > 0)
        {
            TextModule r = ScriptableObject.CreateInstance<TextModule>();
            r.txtContent = allnextParts[0].text;
            r.delayBeforeSend = allnextParts[0].delay;
            r.previousModule = previousModule;
            r.nextModule = nextModule;
            r.SetModuleID(moduleID);
            r.subID = subID + 1;
            r.UIObjectTemplate = getUIObject();

            //allnextParts.RemoveAt(0);
            r.allnextParts = allnextParts;

            return r;
        } else { return nextModule; }
    }
}
