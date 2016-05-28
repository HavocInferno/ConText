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
    private SerializedProperty successModule, failureModule, tieModule;

    private TicTacToe mod;
    private GUIContent textLabel, prevMLabel, succMLabel, failMLabel, tieMLabel, modIDLabel, subIDLabel, charLabel, logLabel;

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
        tieMLabel = new GUIContent("TNext module", "is usually set automatically when using this Inspector's Create button");
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
        EditorGUILayout.LabelField("ID: " + mod.seqID + "(seq) " + mod.branchID + "(branch) " + mod.hierarchyID + "(hierarchy)");
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
        //success module selection
        ModuleBlueprint succMod = (ModuleBlueprint)EditorGUILayout.ObjectField(succMLabel, mod.moduleSuccess, typeof(ModuleBlueprint), false);
        if (succMod != null)
        {
            mod.moduleSuccess = succMod;
        }

        //failure module selection
        ModuleBlueprint failMod = (ModuleBlueprint)EditorGUILayout.ObjectField(failMLabel, mod.moduleFailure, typeof(ModuleBlueprint), false);
        if (failMod != null)
        {
            mod.moduleFailure = failMod;
        }

        //tie module selection
        ModuleBlueprint tieMod = (ModuleBlueprint)EditorGUILayout.ObjectField(tieMLabel, mod.moduleTie, typeof(ModuleBlueprint), false);
        if (tieMod != null)
        {
            mod.moduleTie = tieMod;
        }

        EditorGUILayout.Space();

        //create a success/failure/tie module and give it a fitting set of IDs
        //{TODO}: selection list where the user can select which *type* of module is up next
        if (GUILayout.Button("Create succ module"))
        {
            mod.moduleSuccess = TextModuleInspector.CreateModule();
            ((TextModule)mod.moduleSuccess).txtContent = "[textless]";
            mod.moduleSuccess.sendingCharacter = mod.sendingCharacter;

            mod.moduleSuccess.previousModule = mod;
            if (mod.previousModule == null)
            {
                mod.seqID = mod.branchID = mod.hierarchyID = 0;
            }
            mod.moduleSuccess.seqID = 0;
            mod.moduleSuccess.branchID = 0;
            mod.moduleSuccess.hierarchyID = mod.hierarchyID + 1;
        }

        if (GUILayout.Button("Create fail module"))
        {
            mod.moduleFailure = TextModuleInspector.CreateModule();
            ((TextModule)mod.moduleFailure).txtContent = "[textless]";
            mod.moduleFailure.sendingCharacter = mod.sendingCharacter;

            mod.moduleFailure.previousModule = mod;
            if (mod.previousModule == null)
            {
                mod.seqID = mod.branchID = mod.hierarchyID = 0;
            }
            mod.moduleFailure.seqID = 0;
            mod.moduleFailure.branchID = 1;
            mod.moduleFailure.hierarchyID = mod.hierarchyID + 1;
        }

        if (GUILayout.Button("Create tie module"))
        {
            mod.moduleTie = TextModuleInspector.CreateModule();
            ((TextModule)mod.moduleTie).txtContent = "[textless]";
            mod.moduleTie.sendingCharacter = mod.sendingCharacter;

            mod.moduleTie.previousModule = mod;
            if (mod.previousModule == null)
            {
                mod.seqID = mod.branchID = mod.hierarchyID = 0;
            }
            mod.moduleTie.seqID = 0;
            mod.moduleTie.branchID = 2;
            mod.moduleTie.hierarchyID = mod.hierarchyID + 1;
        }
    }
}
