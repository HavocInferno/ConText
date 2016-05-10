using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(TicTacToe))]
public class TicTacInspector : Editor
{

    //private SerializedProperty modID;
    private SerializedProperty prevModule;
    private SerializedProperty textContent;
    private SerializedProperty successModule, failureModule;

    private TicTacToe mod;
    private GUIContent textLabel, prevMLabel, succMLabel, failMLabel, modIDLabel, subIDLabel, charLabel, logLabel;

    [MenuItem("Assets/Create/ConText Framework/Modules/Minigames/TicTacToe")]
    public static ModuleBlueprint CreateModule()
    {
        return AssetCreator.CreateCustomAsset<TicTacToe>();
    }

    public void OnEnable()
    {
        //modID = serializedObject.FindProperty("moduleID");
        textContent = serializedObject.FindProperty("txtContent");

        mod = target as TicTacToe;

        textLabel = new GUIContent("Text message (incl. markup)");
        prevMLabel = new GUIContent("Previous module", "is usually set automatically when using this Inspector's Create button");
        succMLabel = new GUIContent("SNext module", "is usually set automatically when using this Inspector's Create button");
        failMLabel = new GUIContent("FNext module", "is usually set automatically when using this Inspector's Create button");
        modIDLabel = new GUIContent("Module ID", "unique ID, automatically generated when using this Inspector's Create button");
        subIDLabel = new GUIContent("Sub ID", "unique ID, automatically generated when using this Inspector's Create button");
        charLabel = new GUIContent("Character", "which character is sending this?");
        logLabel = new GUIContent("Log", "log");
    }

    public virtual void drawTextField(int height)
    {
        EditorGUILayout.PropertyField(textContent, textLabel, GUILayout.MaxHeight(height));
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        //previous module selection
        ModuleBlueprint prevMod = (ModuleBlueprint)EditorGUILayout.ObjectField(prevMLabel, mod.previousModule, typeof(ModuleBlueprint), false);
        if (prevMod != null || true)
        {
            mod.previousModule = prevMod;
        }
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Module ID: " + mod.moduleID);
        EditorGUILayout.LabelField("Sub ID: " + mod.subID);
        EditorGUILayout.EndHorizontal();

        //character sending the message -> change to dropdown (enum of all characters, then determine forward/backward?)
        mod.sendingCharacter = (Character)EditorGUILayout.ObjectField(charLabel, mod.sendingCharacter, typeof(Character), false);
        EditorGUILayout.HelpBox("testus", MessageType.Info);

        //text input
        drawTextField(500);

        ModuleSpecific();

        if (GUILayout.Button("Add Log Entry"))
        {
            mod.log = LogEntryInspector.CreateEntry();
        }
        mod.log = (LogEntry)EditorGUILayout.ObjectField(logLabel, mod.log, typeof(LogEntry), false);

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(mod);
    }

    public virtual void ModuleSpecific()
    {
        //next module selection
        ModuleBlueprint succMod = (ModuleBlueprint)EditorGUILayout.ObjectField(succMLabel, mod.moduleSuccess, typeof(ModuleBlueprint), false);
        if (succMod != null)
        {
            mod.moduleSuccess = succMod;
        }

        //next module selection
        ModuleBlueprint failMod = (ModuleBlueprint)EditorGUILayout.ObjectField(failMLabel, mod.moduleFailure, typeof(ModuleBlueprint), false);
        if (failMod != null)
        {
            mod.moduleFailure = failMod;
        }

        EditorGUILayout.Space();

        //create a new module and give it a fitting set of IDs
        //{TODO}: selection list where the user can select which *type* of module is up next
        if (GUILayout.Button("Create succ module"))
        {
            mod.moduleSuccess = CreateModule();
            mod.moduleSuccess.previousModule = mod;
            if (mod.previousModule == null)
            {
                mod.moduleID = 0;
                mod.subID = -1;
            }
            if (mod.subID > -1)
            {
                mod.moduleSuccess.moduleID = ((mod.moduleID == 0 ? 1 : mod.moduleID) * 100 + mod.subID * 10) + 1;
            }
            else
            {
                mod.moduleSuccess.moduleID = mod.moduleID + 1;
            }
            mod.moduleSuccess.subID = -1;
        }

        if (GUILayout.Button("Create fail module"))
        {
            mod.moduleFailure = CreateModule();
            mod.moduleFailure.previousModule = mod;
            if (mod.previousModule == null)
            {
                mod.moduleID = 0;
                mod.subID = -1;
            }
            if (mod.subID > -1)
            {
                mod.moduleFailure.moduleID = ((mod.moduleID == 0 ? 1 : mod.moduleID) * 100 + mod.subID * 10) + 1;
            }
            else
            {
                mod.moduleFailure.moduleID = mod.moduleID + 1;
            }
            mod.moduleFailure.subID = -1;
        }
    }
}
