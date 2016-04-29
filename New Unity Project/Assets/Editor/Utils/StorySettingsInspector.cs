using UnityEngine;
using UnityEditor;
using System.Collections;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(StorySettings))]
public class StorySettingsInspector : Editor {

    private StorySettings stt;

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
        DrawDefaultInspector();
        serializedObject.Update();

       // serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Add a character"))
        {
            stt.addChar(CreateCharacter());
        }

        //needs a custom view of the characters with a button beside each to delete. on delete -> adjust string array and list in storysettings

        EditorUtility.SetDirty(stt);
    }
}
