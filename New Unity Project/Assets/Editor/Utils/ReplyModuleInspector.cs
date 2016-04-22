using UnityEngine;
using UnityEditor;
using System.Collections;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(ReplyModule))]
public class ReplyModuleInspector : TextModuleInspector {

    private GUIContent repliesLabel;
    [MenuItem("Assets/Create/ConText Framework/Modules/Reply Module")]
    public static ModuleBlueprint CreateModule()
    {
        return AssetCreator.CreateCustomAsset<ReplyModule>();
    }

    void OnEnable()
    {
        base.OnEnable();

        repliesLabel = new GUIContent("Replies");
    }

    public override void drawTextField(int height)
    {
        base.drawTextField(200);
    }

    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();

        base.OnInspectorGUI();

        EditorGUILayout.IntField(repliesLabel, 5);
    }
}
