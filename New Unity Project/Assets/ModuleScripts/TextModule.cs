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
}
