using UnityEngine;
using UnityEditor;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(TextModule))]
public class TextModuleInspector : ModuleInspectorAncestor
{
    private SerializedProperty textContent;

    [MenuItem("Assets/Create/ConText Framework/Modules/Text Module")]
    public static ModuleBlueprint CreateModuleManual()
    {
        return AssetCreator.CreateCustomAsset<TextModule>(null);
    }

    public static ModuleBlueprint CreateModule(string name)
    {
        return AssetCreator.CreateCustomAsset<TextModule>(name);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        mod = target as TextModule;
        textContent = serializedObject.FindProperty("txtContent");
    }

    public virtual void drawTextField(int height)
    {
        EditorGUILayout.BeginHorizontal();
        if (((TextModule)mod).txtContent.Length > 0)
        {
            reqStyle.normal.textColor = Color.green;
        }
        GUILayout.Label(required, reqStyle);
        reqStyle.normal.textColor = Color.red;

        EditorGUILayout.PropertyField(textContent, contentLabel, GUILayout.MaxHeight(height));

        EditorGUILayout.EndHorizontal();
    }

    public override void PartMessage()
    {
        EditorGUILayout.LabelField("message ID: " + mod.seqID + "(seq) " + mod.branchID + "(branch) " + mod.hierarchyID + "(hierarchy)");
        EditorGUILayout.Space();

        //character sending the message -> change to dropdown (enum of all characters, then determine forward/backward?)
        if (mod.sendingCharacter == null)
            EditorGUILayout.HelpBox("No character assigned!", MessageType.Error);
        EditorGUILayout.BeginHorizontal();
        if (mod.sendingCharacter != null)
        {
            reqStyle.normal.textColor = Color.green;
        }
        GUILayout.Label(required, reqStyle);
        reqStyle.normal.textColor = Color.red;
        mod.sendingCharacter = (Character)EditorGUILayout.ObjectField(charLabel, mod.sendingCharacter, typeof(Character), false);
        EditorGUILayout.EndHorizontal();

        mod.delayBeforeSend = EditorGUILayout.FloatField("Delay before sending (seconds)", mod.delayBeforeSend);
        EditorGUILayout.Space();

        //text input
        drawTextField(500);
    }
}
