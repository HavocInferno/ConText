using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(TextModule))]
public class TextModuleInspector : Editor {

    //private SerializedProperty modID;
    private SerializedProperty prevModule;
    private SerializedProperty character;
    private SerializedProperty textContent;
    private SerializedProperty nextModule;

    private TextModule mod;
    private GUIContent textLabel, prevMLabel, nextMLabel, modIDLabel;

    [MenuItem("Assets/Create/TextModule")]
    public static ModuleBlueprint CreateModule()
    {
        return ModuleCreator.CreateModuleAsset<TextModule>();
    }

    void OnEnable()
    {
        //modID = serializedObject.FindProperty("moduleID");
        textContent = serializedObject.FindProperty("txtContent");

        mod = target as TextModule;

        textLabel = new GUIContent("Text message (incl. markup)");
        prevMLabel = new GUIContent("Previous module", "is usually set automatically when using this Inspector's Create button");
        nextMLabel = new GUIContent("Next module", "is usually set automatically when using this Inspector's Create button");
        modIDLabel = new GUIContent("Module ID", "unique ID, automatically generated when using this Inspector's Create button");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        //previous module selection
        ModuleBlueprint prevMod = (ModuleBlueprint)EditorGUILayout.ObjectField(prevMLabel, mod.previousModule, typeof(ModuleBlueprint), false);
        if(prevMod != null || true)
        {
            mod.previousModule = prevMod;
        }
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.IntField(modIDLabel, mod.moduleID);

        //text input
        EditorGUILayout.PropertyField(textContent, textLabel, GUILayout.MaxHeight(500));

        //next module selection
        ModuleBlueprint nextMod = (ModuleBlueprint)EditorGUILayout.ObjectField(nextMLabel, mod.nextModule, typeof(ModuleBlueprint), false);
        if (nextMod != null)
        {
            mod.nextModule = nextMod;
        }
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        //GUIStyle style = new GUIStyle(GUI.skin.button);
        if (GUILayout.Button("Create next module"))
        {
            mod.nextModule = CreateModule();
            mod.nextModule.previousModule = mod;
            if(mod.previousModule == null)
            {
                mod.moduleID = 0;
            }
            mod.nextModule.moduleID = mod.moduleID +1;
        }

        EditorUtility.SetDirty(mod);
    }
}
