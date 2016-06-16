using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(TextModule))]
public class TextModuleInspector : ModuleInspectorAncestor
{

    //private SerializedProperty modID;
    private SerializedProperty prevModule;
    private SerializedProperty textContent;
    private SerializedProperty nextModule;

    private TextModule mod;
    protected ModuleManager.ModuleTypes nextModType;
    private GUIContent textLabel, prevMLabel, nextMLabel, modIDLabel, subIDLabel, charLabel, logLabel;

    [MenuItem("Assets/Create/ConText Framework/Modules/Text Module")]
    public static ModuleBlueprint CreateModule(string name)
    {
        return AssetCreator.CreateCustomAsset<TextModule>(name);
    }

    public void OnEnable()
    {
        //modID = serializedObject.FindProperty("moduleID");
        textContent = serializedObject.FindProperty("txtContent");

        mod = target as TextModule;

        textLabel = new GUIContent("Text message (incl. markup)");
        prevMLabel = new GUIContent("Previous module", "is usually set automatically when using this Inspector's Create button");
        nextMLabel = new GUIContent("Next module", "is usually set automatically when using this Inspector's Create button");
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
        if(prevMod != null)
        {
            mod.previousModule = prevMod;
            prevMod.nextModule = mod;
            fixNextIDs();
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
            mod.log = LogEntryInspector.CreateEntry(null);
        }
        mod.log = (LogEntry)EditorGUILayout.ObjectField(logLabel, mod.log, typeof(LogEntry), false);

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(mod);
    }

    public virtual void ModuleSpecific()
    {
        //next module selection
        ModuleBlueprint nextMod = (ModuleBlueprint)EditorGUILayout.ObjectField(nextMLabel, mod.nextModule, typeof(ModuleBlueprint), false);
        if (nextMod != null)
        {
            mod.nextModule = nextMod;
        }

        EditorGUILayout.Space();

        //create a new module and give it a fitting set of IDs
        //{TODO}: selection list where the user can select which *type* of module is up next
        nextModType = (ModuleManager.ModuleTypes)EditorGUILayout.Popup("Next module type", (int)nextModType, ModuleManager.m_ModuleTypeEnumDescriptions);

        if (GUILayout.Button("Create next module (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
        {
            mod.nextModule = createNextModule(nextModType, mod, mod.seqID + 1, mod.branchID, mod.hierarchyID, mod.subpartID);
            /*mod.nextModule = CreateModule();
            mod.nextModule.previousModule = mod;
            ((TextModule)mod.nextModule).txtContent = "[textless]";
            mod.nextModule.sendingCharacter = mod.sendingCharacter;
            if (mod.previousModule == null)
            {
                mod.seqID = mod.branchID = mod.hierarchyID = 0;
            }
            mod.nextModule.seqID = mod.seqID + 1;
            mod.nextModule.branchID = mod.branchID;
            mod.nextModule.hierarchyID = mod.hierarchyID;*/
        }
    }

    public void fixNextIDs()
    {
        if (mod.seqID == -1 || mod.branchID == -1 || mod.hierarchyID == -1)
        {
            mod.seqID = mod.previousModule.seqID + 1;
            mod.branchID = mod.previousModule.branchID;
            mod.hierarchyID = mod.previousModule.hierarchyID;
        }
    }
}
