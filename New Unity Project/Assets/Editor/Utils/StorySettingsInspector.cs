using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(StorySettings))]
public class StorySettingsInspector : Editor {

    private StorySettings stt;

    private List<bool> showInfos;

	[MenuItem("Assets/Create/ConText Framework/Story Settings")]
    public static StorySettings Create()
    {
        return AssetCreator.CreateCustomAsset<StorySettings>(null);
    }

    [MenuItem("Assets/Create/ConText Framework/Character")]
    public static Character CreateCharacter()
    {
        return AssetCreator.CreateCustomAsset<Character>(null);
    }

    void OnEnable()
    {
        stt = target as StorySettings;

        showInfos = new List<bool>();
        for(int i = 0; i < stt.characters.Count; i++)
        {
            showInfos.Add(false);
        }
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        GUILayout.Label("General story settings", EditorStyles.boldLabel);
        stt.defaultMsgSound = (AudioClip)EditorGUILayout.ObjectField("Default message sound", stt.defaultMsgSound, typeof(AudioClip), false);
        stt.backgroundTrack = (AudioClip)EditorGUILayout.ObjectField("Background track", stt.backgroundTrack, typeof(AudioClip), false);
        GUIStyle mLww = new GUIStyle(EditorStyles.miniLabel);
        mLww.wordWrap = true;
        GUILayout.Label("The default message sound will be played whenever a message is fired/arrives. The background track will continuously play while the game is active.", mLww);
        GUILayout.Space(20);

        GUILayout.Label("Character List", EditorStyles.boldLabel);
        /*display a list of characters with their associated assets as well as (label) each character name*/
        for (int i = 0; i < stt.characters.Count; i++)
        {
            EditorGUILayout.BeginVertical();

            EditorGUILayout.BeginHorizontal();
            stt.characters[i] = (Character)EditorGUILayout.ObjectField(stt.characters[i], typeof(Character), false);
            EditorGUILayout.LabelField("Name: " + (stt.characters[i] != null ? stt.characters[i].characterName : ""));

            if (GUILayout.Button("Go to asset"))
            {
                Selection.activeObject = stt.characters[i];
            }

            if (GUILayout.Button("Delete (irreversible)"))
            {
                Character ch = stt.characters[i];
                Debug.Log(ch.ToString() + " deleted? -> " + AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(ch)));
                stt.characters.RemoveAt(i);
                showInfos.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();

            showInfos[i] = EditorGUILayout.Foldout(showInfos[i], showInfos[i] ? "Hide" : "Show");
            if (showInfos[i])
            {
                Character ch = stt.characters[i];
                EditorGUILayout.LabelField("Character...");
                ch.charID = EditorGUILayout.IntField("ID", ch.charID);
                ch.characterName = EditorGUILayout.TextField("Name", ch.characterName); AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(ch), "ch_" + ch.characterName);
                EditorGUILayout.Space();
                ch.blobColor = EditorGUILayout.ColorField("msg color", ch.blobColor);
                ch.blobBackground = (Sprite)EditorGUILayout.ObjectField("msg background image", ch.blobBackground, typeof(Sprite), false);
                ch.alignment = (Character.blobAlignment)EditorGUILayout.EnumPopup("msg alignment", ch.alignment);
            }
            EditorGUILayout.EndVertical();
            GUILayout.Label("--------------------------------------------------------------------------------------------------------------------------------", EditorStyles.centeredGreyMiniLabel);
            GUILayout.Space(10);
        }

        if (GUILayout.Button("Add a character", GUILayout.MaxWidth(250f)))
        {
            showInfos.Add(false);
            stt.addChar(CreateCharacter());
            Selection.activeObject = stt;
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(stt);
    }
}
