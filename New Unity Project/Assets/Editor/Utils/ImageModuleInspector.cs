using UnityEngine;
using UnityEditor;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(ImageModule))]
public class ImageModuleInspector : ModuleInspectorAncestor
{
    [MenuItem("Assets/Create/ConText Framework/Modules/Image Module")]
    public static ModuleBlueprint CreateModuleManual()
    {
        return AssetCreator.CreateCustomAsset<ImageModule>(null);
    }

    public static ModuleBlueprint CreateModule(string name)
    {
        return AssetCreator.CreateCustomAsset<ImageModule>(name);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        mod = target as ImageModule;

        contentLabel = new GUIContent("Image content");
    }

    public override void PartMessage()
    {
        EditorGUILayout.LabelField("message ID: " + mod.seqID + "(seq) " + mod.branchID + "(branch) " + mod.hierarchyID + "(hierarchy)");
        EditorGUILayout.Space();

        mod.sendingCharacter = (Character)EditorGUILayout.ObjectField(charLabel, mod.sendingCharacter, typeof(Character), false);
        mod.delayBeforeSend = EditorGUILayout.FloatField("Delay before sending (seconds)", mod.delayBeforeSend);
        base.PartMessage();

        ((ImageModule)mod).imgContent = (Sprite)EditorGUILayout.ObjectField(contentLabel, ((ImageModule)mod).imgContent, typeof(Sprite), false, GUILayout.ExpandHeight(true));
    }
}
