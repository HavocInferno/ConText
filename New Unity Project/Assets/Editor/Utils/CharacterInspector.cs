using UnityEngine;
using System.Collections;
using UnityEditor;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

//custom editor for Characters, so far no specific changes
[CustomEditor(typeof(Character))]
public class CharacterInspector : Editor
{

    private Character ch;

    void OnEnable()
    {
        ch = target as Character;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        ch.name = ch.characterName;
        AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(ch), "ch_" + ch.characterName);

        EditorUtility.SetDirty(ch);
    }
}
