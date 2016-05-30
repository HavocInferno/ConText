using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class TextModule : ModuleBlueprint {

    [TextArea(5, 15)]
    public string txtContent;

    //private Text UIContent;
    //this has the module's text parsed
    private List<TextParser.ParsedChunk> allnextParts;

    public override GameObject getUIObject()
    {
        return UIObjectTemplate;
    }

    public override void setContent(GameObject UIObjectInstance)
    {
        base.setContent(UIObjectInstance);

        Debug.Log("setting content for ID " + seqID + "s-" + branchID + "b-" + hierarchyID + "h-" + subpartID + "sp");
        Text UIContent = null;
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

        UIContent.font = Unify.Instance.UIMng.UISettings.moduleTextFont;
        UIContent.fontSize = Unify.Instance.UIMng.UISettings.moduleTextFontSize;
        UIContent.text = (sendingCharacter != null ? sendingCharacter.characterName : "") + "@" + System.DateTime.Now.ToString() + ": " + allnextParts[0].text;

        //float canvWidth = Unify.Instance.UIMng.UIWrap.canvas.pixelRect.width;
        //LayoutElement lelem = UIObjectInstance.GetComponentInChildren<ModuleUIHelper>().TextContainer.GetComponentInParent<LayoutElement>();
        //lelem.minWidth = canvWidth - 50f;
        //lelem.minHeight =
        //((UIContent.text.Length * Unify.Instance.UIMng.UIWrap.letterWidth) / lelem.minWidth) * UIContent.fontSize;

        //Debug.Log(UIContent.GetComponent<RectTransform>().);
        //Debug.Log(UIObjectInstance.GetComponentInChildren<ModuleUIHelper>().TextContainer.GetComponentInChildren<RectTransform>().sizeDelta[0]);

        if (sendingCharacter.blobBackground != null)
        {
            Image tmp = UIObjectInstance.GetComponentInChildren<Image>();
            tmp.sprite = sendingCharacter.blobBackground;
            tmp.color = Color.white;
            tmp.enabled = true;
        }

        delayBeforeSend = allnextParts[0].delay;
        allnextParts.RemoveAt(0);

        Unify.Instance.LogMng.fireLog(log);
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
            r.SetModuleID(seqID, branchID, hierarchyID);
            r.subpartID = subpartID + 1;
            r.UIObjectTemplate = getUIObject();
            r.sendingCharacter = sendingCharacter;

            r.allnextParts = allnextParts;

            return r;
        } else { return nextModule; }
    }
}
