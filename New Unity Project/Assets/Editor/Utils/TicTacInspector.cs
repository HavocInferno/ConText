using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(TicTacToe))]
public class TicTacInspector : ModuleInspectorAncestor
{

    //private SerializedProperty modID;
    private SerializedProperty prevModule;
    private SerializedProperty textContent;
    private SerializedProperty successModule, failureModule, tieModule;

    private TicTacToe mod;
    private ModuleManager.ModuleTypes nextModType;
    private GUIContent textLabel, prevMLabel, succMLabel, failMLabel, tieMLabel, modIDLabel, subIDLabel, charLabel, logLabel;

    [MenuItem("Assets/Create/ConText Framework/Modules/Minigames/TicTacToe")]
    public static ModuleBlueprint CreateModule(string name)
    {
        return AssetCreator.CreateCustomAsset<TicTacToe>(name);
    }

    public void OnEnable()
    {
        //modID = serializedObject.FindProperty("moduleID");
        textContent = serializedObject.FindProperty("txtContent");

        mod = target as TicTacToe;

        textLabel = new GUIContent("Text message (incl. markup)");
        prevMLabel = new GUIContent("Previous module", "is usually set automatically when using this Inspector's Create button");
        succMLabel = new GUIContent("Win module", "is usually set automatically when using this Inspector's Create button");
        failMLabel = new GUIContent("Lose module", "is usually set automatically when using this Inspector's Create button");
        tieMLabel = new GUIContent("Tie module", "is usually set automatically when using this Inspector's Create button");
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

        showHints = EditorGUILayout.Toggle("Show hints", showHints);

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Previous module", EditorStyles.boldLabel);

        //previous module selection
        EditorGUILayout.BeginHorizontal();
        ModuleBlueprint prevMod = (ModuleBlueprint)EditorGUILayout.ObjectField(prevMLabel, mod.previousModule, typeof(ModuleBlueprint), false);
        if (prevMod != null || true)
        {
            mod.previousModule = prevMod;
        }
        EditorGUILayout.LabelField(getShortDesc(prevMod), GUILayout.MaxWidth(getShortDesc(prevMod).Length * 10.0f));
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Message properties", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("ID: " + mod.seqID + "(seq) " + mod.branchID + "(branch) " + mod.hierarchyID + "(hierarchy)");

        //character sending the message -> change to dropdown (enum of all characters, then determine forward/backward?)
        mod.sendingCharacter = (Character)EditorGUILayout.ObjectField(charLabel, mod.sendingCharacter, typeof(Character), false);

        //text input
        drawTextField(500);

        ModuleSpecific();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Log", EditorStyles.boldLabel);
        if (showHints)
            EditorGUILayout.HelpBox("The log entry that will be added or updated when this module is triggered.", MessageType.Info);

        if (GUILayout.Button("Add Log Entry"))
        {
            mod.log = LogEntryInspector.CreateEntry(null);
        }
        mod.log = (LogEntry)EditorGUILayout.ObjectField(logLabel, mod.log, typeof(LogEntry), false);

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(mod);
    }

    public virtual void ModuleSpecific()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Next modules", EditorStyles.boldLabel);
        if (showHints)
            EditorGUILayout.HelpBox("The modules being triggered upon win/lose/tie game outcome.", MessageType.Info);

        nextModType = (ModuleManager.ModuleTypes)EditorGUILayout.Popup("Module type", (int)nextModType, ModuleManager.m_ModuleTypeEnumDescriptions);
        EditorGUILayout.Space();

        //success module selection
        EditorGUILayout.BeginHorizontal();
        ModuleBlueprint succMod = (ModuleBlueprint)EditorGUILayout.ObjectField(succMLabel, mod.moduleSuccess, typeof(ModuleBlueprint), false);
        if (succMod != null)
        {
            mod.moduleSuccess = succMod;
        }
        EditorGUILayout.LabelField(getShortDesc(succMod), GUILayout.MaxWidth(getShortDesc(succMod).Length * 10.0f));
        if (succMod == null)
        {
            if (GUILayout.Button("+ (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
            {
                mod.moduleSuccess = createNextModule(nextModType, mod, 0, 0, mod.hierarchyID + 1, mod.subpartID);
            }
        }
        EditorGUILayout.EndHorizontal();

        //failure module selection
        EditorGUILayout.BeginHorizontal();
        ModuleBlueprint failMod = (ModuleBlueprint)EditorGUILayout.ObjectField(failMLabel, mod.moduleFailure, typeof(ModuleBlueprint), false);
        if (failMod != null)
        {
            mod.moduleFailure = failMod;
        }
        EditorGUILayout.LabelField(getShortDesc(failMod), GUILayout.MaxWidth(getShortDesc(failMod).Length * 10.0f));
        if (failMod == null)
        {
            if (GUILayout.Button("+ (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
            {
                mod.moduleFailure = createNextModule(nextModType, mod, 0, 0, mod.hierarchyID + 1, mod.subpartID);
            }
        }
        EditorGUILayout.EndHorizontal();

        //tie module selection
        EditorGUILayout.BeginHorizontal();
        ModuleBlueprint tieMod = (ModuleBlueprint)EditorGUILayout.ObjectField(tieMLabel, mod.moduleTie, typeof(ModuleBlueprint), false);
        if (tieMod != null)
        {
            mod.moduleTie = tieMod;
        }
        EditorGUILayout.LabelField(getShortDesc(tieMod), GUILayout.MaxWidth(getShortDesc(tieMod).Length * 10.0f));
        if (tieMod == null)
        {
            if (GUILayout.Button("+ (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
            {
                mod.moduleTie = createNextModule(nextModType, mod, 0, 0, mod.hierarchyID + 1, mod.subpartID);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        //create a success/failure/tie module and give it a fitting set of IDs
        //{TODO}: selection list where the user can select which *type* of module is up next
        /*nextModType = (ModuleManager.ModuleTypes)EditorGUILayout.Popup("Module type", (int)nextModType, ModuleManager.m_ModuleTypeEnumDescriptions);
        if (GUILayout.Button("Create succ module (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
        {
            mod.moduleSuccess = createNextModule(nextModType, mod, 0, 0, mod.hierarchyID + 1, mod.subpartID);
        }

        if (GUILayout.Button("Create fail module (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
        {
            mod.moduleFailure = createNextModule(nextModType, mod, 0, 1, mod.hierarchyID + 1, mod.subpartID);
        }

        if (GUILayout.Button("Create tie module (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
        {
            mod.moduleTie = createNextModule(nextModType, mod, 0, 2, mod.hierarchyID + 1, mod.subpartID);
        }*/
    }
}
