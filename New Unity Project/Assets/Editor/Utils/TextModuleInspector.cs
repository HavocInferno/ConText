using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(TextModule))]
public class TextModuleInspector : Editor {

    private SerializedProperty prevModule;
    private SerializedProperty character;
    private SerializedProperty textContent;
    private SerializedProperty nextModule;

    private TextModule mod;

    [MenuItem("Assets/Create/TextModule")]
    public static ModuleBlueprint CreateModule()
    {
        return ModuleCreator.CreateModuleAsset<TextModule>();
    }

    void OnEnable()
    {
        textContent = serializedObject.FindProperty("txtContent");

        mod = target as TextModule;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        //previous module selection
        ModuleBlueprint prevMod = (ModuleBlueprint)EditorGUILayout.ObjectField("prev module", mod.previousModule, typeof(ModuleBlueprint), false);
        if(prevMod != null || true)
        {
            mod.previousModule = prevMod;
        }
        serializedObject.ApplyModifiedProperties();

        //EditorGUILayout.EnumPopup(character, );
        //text input
        EditorGUILayout.PropertyField(textContent, GUILayout.MaxHeight(500));

        //next module selection
        ModuleBlueprint nextMod = (ModuleBlueprint)EditorGUILayout.ObjectField("next module", mod.nextModule, typeof(ModuleBlueprint), false);
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
        }

        EditorUtility.SetDirty(mod);
    }
}
