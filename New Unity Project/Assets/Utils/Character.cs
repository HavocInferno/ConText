using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class Character : ScriptableObject {

    public int charID;

    [System.Serializable]
    public enum blobAlignment
    {
        LEFT, 
        RIGHT,
        CENTER,
        NONE
    };

    public string characterName = "char";
    public blobAlignment alignment = blobAlignment.NONE;

    public Color blobColor;
    public Sprite blobBackground;
}
