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

    GUIStyle reqStyle;
    string required;
    bool showSectionPrev, showSectionMsg, showSectionLog, showSectionNext;

    [MenuItem("Assets/Create/ConText Framework/Modules/Text Module")]
    public static ModuleBlueprint CreateModuleManual()
    {
        return AssetCreator.CreateCustomAsset<TextModule>(null);
    }

    public static ModuleBlueprint CreateModule(string name)
    {
        return AssetCreator.CreateCustomAsset<TextModule>(name);
    }

    public void OnEnable()
    {
        //modID = serializedObject.FindProperty("moduleID");
        textContent = serializedObject.FindProperty("txtContent");

        mod = target as TextModule;

        textLabel = new GUIContent("Text message");
        prevMLabel = new GUIContent("Previous module", "is usually set automatically when using this Inspector's Create button");
        nextMLabel = new GUIContent("Next module asset", "is usually set automatically when using this Inspector's Create button");
        modIDLabel = new GUIContent("Module ID", "unique ID, automatically generated when using this Inspector's Create button");
        subIDLabel = new GUIContent("Sub ID", "unique ID, automatically generated when using this Inspector's Create button");
        charLabel = new GUIContent("Character", "which character is sending this?");
        logLabel = new GUIContent("Log entry", "log");

        showSectionPrev = showSectionMsg = showSectionLog = showSectionNext = false;
    }

    public virtual void drawTextField(int height)
    {
        EditorGUILayout.BeginHorizontal();
        if (mod.txtContent.Length > 0)
        {
            reqStyle.normal.textColor = Color.green;
        }
        GUILayout.Label(required, reqStyle);
        reqStyle.normal.textColor = Color.red;

        EditorGUILayout.PropertyField(textContent, textLabel, GUILayout.MaxHeight(height));

        EditorGUILayout.EndHorizontal();
    }

    public override void OnInspectorGUI()
    {
        reqStyle = new GUIStyle(GUI.skin.label);
        required = "(*)";
        reqStyle.normal.textColor = Color.red;
        reqStyle.fixedWidth = 20.0f;

        //base.OnInspectorGUI();
        serializedObject.Update();

        GUILayout.Label("This is a " + mod.GetType().ToString(), EditorStyles.boldLabel);
        showHints = EditorGUILayout.Toggle("Show hints", showHints);
        GUILayout.Label("Fields labeled with (*) need to be set in order for the game to work.");

        if (mod.previousModule != null)
        {
            if(mod.previousModule.sendingCharacter == null)
            {
                GUIStyle errorStyle = new GUIStyle(GUI.skin.label);
                errorStyle.normal.textColor = Color.red;
                errorStyle.wordWrap = true;
                GUILayout.Label("The previous module has no character assigned. The contents of this module cannot be displayed as long as that is the case.", errorStyle);

                GUIStyle errorbStyle = new GUIStyle(GUI.skin.button);
                errorbStyle.normal.textColor = Color.red;
                if (GUILayout.Button("Go to previous to fix", errorbStyle))
                    Selection.activeObject = mod.previousModule;

                return;
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Previous module", EditorStyles.boldLabel);
        showSectionPrev = EditorGUILayout.Foldout(showSectionPrev, showSectionPrev ? "Hide" : "Show");

        if (showSectionPrev)
        {
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
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Message properties", EditorStyles.boldLabel);
        showSectionMsg = EditorGUILayout.Foldout(showSectionMsg, showSectionMsg ? "(*) Hide" : "(*) Show");

        if (showSectionMsg)
        {
            EditorGUILayout.LabelField("message ID: " + mod.seqID + "(seq) " + mod.branchID + "(branch) " + mod.hierarchyID + "(hierarchy)");
            EditorGUILayout.Space();

            //character sending the message -> change to dropdown (enum of all characters, then determine forward/backward?)
            if(mod.sendingCharacter == null)
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

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Log", EditorStyles.boldLabel);
        showSectionLog = EditorGUILayout.Foldout(showSectionLog, showSectionLog ? "Hide" : "Show");

        if (showSectionLog)
        {
            if (showHints)
                EditorGUILayout.HelpBox("The log entry that will be added or updated when this module is triggered.", MessageType.Info);

            if (GUILayout.Button("Add Log Entry"))
            {
                mod.log = LogEntryInspector.CreateEntry(null);
            }
            mod.log = (LogEntry)EditorGUILayout.ObjectField(logLabel, mod.log, typeof(LogEntry), false);
        }

        ModuleSpecific();

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(mod);
    }

    public virtual void ModuleSpecific()
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Next module", EditorStyles.boldLabel);
        showSectionNext = EditorGUILayout.Foldout(showSectionNext, showSectionNext ? "Hide" : "Show");

        if (showSectionNext)
        {
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

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Delete", EditorStyles.boldLabel);
        if (showHints)
            EditorGUILayout.HelpBox("Permanently delete this module. This action cannot be reversed and is final.", MessageType.Info);

        GUIStyle bStyle = new GUIStyle(GUI.skin.button);
        bStyle.normal.textColor = Color.red;
        if (GUILayout.Button("Delete this module (irreversible)", bStyle))
        {
            if(mod.previousModule != null)
                mod.previousModule.nextModule = mod.nextModule;

            if (mod.nextModule != null)
                mod.nextModule.previousModule = mod.previousModule;

            Debug.Log(mod.ToString() + " deleted? -> " + AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(mod)));
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
