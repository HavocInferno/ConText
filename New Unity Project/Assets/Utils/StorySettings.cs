using UnityEngine;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[System.Serializable]
public class StorySettings : ScriptableObject {

    [SerializeField]
    public List<Character> characters = new List<Character>();
    static string[] charNames = new string[0];

    public void addChar(Character nC)
    {
        characters.Add(nC);
        if(characters.Count >= charNames.Length)
        {
            string[] newCN = new string[charNames.Length + 1];
            charNames.CopyTo(newCN, 0);
            newCN[charNames.Length] = nC.characterName;
            charNames = newCN;
        }
    }

    public static string[] getListAsStrings()
    {
        return charNames;
    }
}
