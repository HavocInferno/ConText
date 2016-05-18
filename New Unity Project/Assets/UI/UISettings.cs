using UnityEngine;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class UISettings : ScriptableObject {

    public GameObject[] moduleTemplates;

    public GameObject MenuView, TextView, LogView;
    public StorySettings sSettings;

    public Font moduleTextFont;

    public Color viewBackgroundColor;
    public Font viewTextFont;

    [System.Serializable]
    public class modUIPair
    {
        public string modClassName;
        public GameObject modUITemplate;
    }
    public List<modUIPair> modUITemplates = new List<modUIPair>();
}
