using UnityEngine;
using UnityEditor;
using System.Collections;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(StorySettings))]
public class StorySettingsInspector : Editor {

    private StorySettings stt;

    private GUIContent charsLabel = new GUIContent("Characters");

	[MenuItem("Assets/Create/ConText Framework/Story Settings")]
    public static void Create()
    {
        AssetCreator.CreateCustomAsset<StorySettings>();
    }

    [MenuItem("Assets/Create/ConText Framework/Character")]
    public static Character CreateCharacter()
    {
        return AssetCreator.CreateCustomAsset<Character>();
    }

    void OnEnable()
    {
        stt = target as StorySettings;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        serializedObject.ApplyModifiedProperties();

        /*display a list of characters with their associated assets as well as (label) each character name*/
        EditorGUILayout.LabelField(charsLabel);
        for (int i = 0; i < stt.characters.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            stt.characters[i] = (Character)EditorGUILayout.ObjectField(stt.characters[i], typeof(Character), false);
            EditorGUILayout.LabelField("Name: " + (stt.characters[i] != null ? stt.characters[i].characterName : ""));

            if (GUILayout.Button("Delete"))
            {
                stt.characters.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add a character slot"))
        {
            stt.characters.Add(null);
        }
        if (GUILayout.Button("Add a character"))
        {
            stt.addChar(CreateCharacter());
        }
        EditorGUILayout.EndHorizontal();

        EditorUtility.SetDirty(stt);
    }
}
