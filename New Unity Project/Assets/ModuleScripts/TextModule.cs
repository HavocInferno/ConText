using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextModule : ModuleBlueprint {

    [TextArea(5, 15)]
    public string txtContent;

    private Text UIContent;

    // Use this for initialization
    /* void Start () {
         Debug.Log("dis is a text module");
         base.Start();
     }*/

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
            r.SetModuleID(moduleID); //bad temp fix! need to expand modules dict in ModMng to Pairs for keys in order to save ID + subID as key!
            r.subID = subID + 1;
            r.previousModule = previousModule;
            r.nextModule = nextModule;
            r.UIObject = getUIObject();
            return r;
        } else { return nextModule; }
    }
}
