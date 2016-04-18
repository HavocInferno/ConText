using UnityEngine;
using System.Collections;

public class TextModule : ModuleBlueprint {

    [TextArea(5, 15)]
    public string txtContent;

    // Use this for initialization
    void Start () {
        Debug.Log("dis is a text module");
        base.Start();
	}

    public void setContent(string txt)
    {
        txtContent = txt;
    }
}
