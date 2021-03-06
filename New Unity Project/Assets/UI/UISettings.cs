﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class UISettings : ScriptableObject {

    /*so far, this encompasses just the variables for storing all sorts of UI info.
    However, the functions for applying changes should also be moved here instead of the custom Inspector.*/

    public GameObject[] moduleTemplates;

    public GameObject MenuView, TextView, LogView;
    public Image MenuImage, TextImage, LogImage;

    public StorySettings sSettings;

    public Font moduleTextFont;
    public int moduleTextFontSize;
    public float moduleWidth = 400f;

    public Color viewBackgroundColor;
    public Font viewTextFont;
    public int viewTextFontSize;

    [System.Serializable]
    public class modUIPair
    {
        public string modClassName;
        public GameObject modUITemplate;
    }
    public List<modUIPair> modUITemplates = new List<modUIPair>();

    /*Should add proper clean apply functions here*/
    //Foo
}
