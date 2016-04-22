using UnityEngine;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[System.Serializable]
public class StorySettings : ScriptableObject {

    [SerializeField]
    List<Character> characters = new List<Character>();

    public void addChar(Character nC)
    {
        characters.Add(nC);
    }
}
