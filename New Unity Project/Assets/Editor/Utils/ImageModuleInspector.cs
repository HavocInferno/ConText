using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(ImageModule))]
public class ImageModuleInspector : ModuleInspectorAncestor
{

    //private SerializedProperty modID;
    private SerializedProperty prevModule;
    private SerializedProperty nextModule;

    private ImageModule mod;
    private ModuleManager.ModuleTypes nextModType;
    private GUIContent imgLabel, prevMLabel, nextMLabel, modIDLabel, subIDLabel, charLabel, logLabel;

    [MenuItem("Assets/Create/ConText Framework/Modules/Image Module")]
    public static ModuleBlueprint CreateModuleManual()
    {
        return AssetCreator.CreateCustomAsset<ImageModule>(null);
    }

    public static ModuleBlueprint CreateModule(string name)
    {
        return AssetCreator.CreateCustomAsset<ImageModule>(name);
    }

    public void OnEnable()
    {
        //modID = serializedObject.FindProperty("moduleID");
        //imageContent = serializedObject.FindProperty("imgContent");

        mod = target as ImageModule;

        imgLabel = new GUIContent("Image");
        prevMLabel = new GUIContent("Previous module", "is usually set automatically when using this Inspector's Create button");
        nextMLabel = new GUIContent("Next module", "is usually set automatically when using this Inspector's Create button");
        modIDLabel = new GUIContent("Module ID", "unique ID, automatically generated when using this Inspector's Create button");
        subIDLabel = new GUIContent("Sub ID", "unique ID, automatically generated when using this Inspector's Create button");
        charLabel = new GUIContent("Character", "which character is sending this?");
        logLabel = new GUIContent("Log", "log");
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
        if (prevMod != null)
        {
            mod.previousModule = prevMod;
            prevMod.nextModule = mod;
            fixNextIDs();
        }
        EditorGUILayout.LabelField(getShortDesc(prevMod), GUILayout.MaxWidth(getShortDesc(prevMod).Length * 10.0f));
        if (prevMod != null)
        {
            if (GUILayout.Button("Go to"))
                Selection.activeObject = prevMod;
        }
        EditorGUILayout.EndHorizontal();
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Message properties", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("ID: " + mod.seqID + "(seq) " + mod.branchID + "(branch) " + mod.hierarchyID + "(hierarchy)");

        mod.imgContent = (Sprite)EditorGUILayout.ObjectField(imgLabel, mod.imgContent, typeof(Sprite), false, GUILayout.ExpandHeight(true));

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Log", EditorStyles.boldLabel);
        if (showHints)
            EditorGUILayout.HelpBox("The log entry that will be added or updated when this module is triggered.", MessageType.Info);

        if (GUILayout.Button("Add Log Entry"))
        {
            mod.log = LogEntryInspector.CreateEntry(null);
        }
        mod.log = (LogEntry)EditorGUILayout.ObjectField(logLabel, mod.log, typeof(LogEntry), false);

        ModuleSpecific();

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(mod);
    }

    public virtual void ModuleSpecific()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Next module", EditorStyles.boldLabel);
        if (showHints)
            EditorGUILayout.HelpBox("The module being triggered after this one.", MessageType.Info);


        //next module selection
        EditorGUILayout.BeginHorizontal();
        ModuleBlueprint nextMod = (ModuleBlueprint)EditorGUILayout.ObjectField(nextMLabel, mod.nextModule, typeof(ModuleBlueprint), false);
        if (nextMod != null)
        {
            mod.nextModule = nextMod;
        }
        EditorGUILayout.LabelField(getShortDesc(nextMod), GUILayout.MaxWidth(getShortDesc(nextMod).Length * 10.0f));
        if (nextMod != null)
        {
            if (GUILayout.Button("Go to"))
                Selection.activeObject = nextMod;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        //create a new module and give it a fitting set of IDs
        //selection list where the user can select which *type* of module is up next
        nextModType = (ModuleManager.ModuleTypes)EditorGUILayout.Popup("Next module type", (int)nextModType, ModuleManager.m_ModuleTypeEnumDescriptions);

        if (GUILayout.Button("Create next module (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
        {
            mod.nextModule = createNextModule(nextModType, mod, mod.seqID + 1, mod.branchID, mod.hierarchyID, mod.subpartID);
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
