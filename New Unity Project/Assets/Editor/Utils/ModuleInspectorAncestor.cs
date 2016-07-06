using UnityEngine;
using UnityEditor;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class ModuleInspectorAncestor : Editor {

    public bool showHints = false;
    bool reallyDelete = false;
    protected ModuleBlueprint mod;
    protected SerializedProperty prevModule;
    protected SerializedProperty nextModule;

    protected ModuleManager.ModuleTypes nextModType;
    protected bool showSectionPrev, showSectionMsg, showSectionLog, showSectionNext;
    protected GUIContent contentLabel, prevMLabel, nextMLabel, charLabel, logLabel;
    protected GUIContent partPrevious, partMessage, partLog, partNext, partDelete;
    public GUIStyle reqStyle; public string required;

    public static string getShortDesc(ModuleBlueprint m)
    {
        if (m == null)
        {
            return "(no module specified)";
        }
        else if (m.sendingCharacter == null)
        {
            return "(char. not specified, Error)";
        }

        if(m is TextModule)
        {
            return "(" + m.sendingCharacter.characterName + "; Text)";
        }

        if(m is ImageModule)
        {
            return "(" + m.sendingCharacter.characterName + "; Image)";
        }

        if(m is ReplyModule)
        {
            return "(" + m.sendingCharacter.characterName + "; Reply)";
        }

        if(m is TicTacToe)
        {
            return "(" + m.sendingCharacter.characterName + "; TicTacToe)";
        }

        return "";
    }

    public static ModuleBlueprint createNextModule(ModuleManager.ModuleTypes mtype, ModuleBlueprint thisMod, int newSeqID, int newBranchID, int newHierarchyID, int newSubpartID)
    {
        string newAssetName = "H" + newHierarchyID + ".B" + newBranchID + ".S" + newSeqID;
        switch (mtype)
        {
            case ModuleManager.ModuleTypes.TEXTM:
                TextModule tm = (TextModule)TextModuleInspector.CreateModule(newAssetName + "-TextModule");
                if (thisMod != null)
                {
                    tm.previousModule = thisMod;
                    tm.sendingCharacter = thisMod.sendingCharacter;
                    if (thisMod.previousModule == null)
                    {
                        thisMod.seqID = thisMod.branchID = thisMod.hierarchyID = 0;
                    }
                }
                tm.txtContent = "[textless]";
                tm.seqID = newSeqID;
                tm.branchID = newBranchID;
                tm.hierarchyID = newHierarchyID;
                tm.subpartID = newSubpartID;
                return tm;
            case ModuleManager.ModuleTypes.IMGM:
                ImageModule im = (ImageModule)ImageModuleInspector.CreateModule(newAssetName + "-ImageModule");
                if (thisMod != null)
                {
                    im.previousModule = thisMod;
                    im.sendingCharacter = thisMod.sendingCharacter;
                    if (thisMod.previousModule == null)
                    {
                        thisMod.seqID = thisMod.branchID = thisMod.hierarchyID = 0;
                    }
                }
                im.seqID = newSeqID;
                im.branchID = newBranchID;
                im.hierarchyID = newHierarchyID;
                im.subpartID = newSubpartID;
                return im;
            case ModuleManager.ModuleTypes.REPLYM:
                ReplyModule rm = (ReplyModule)ReplyModuleInspector.CreateModule(newAssetName + "-ReplyModule");
                if (thisMod != null)
                {
                    rm.previousModule = thisMod;
                    rm.sendingCharacter = thisMod.sendingCharacter;
                    if (thisMod.previousModule == null)
                    {
                        thisMod.seqID = thisMod.branchID = thisMod.hierarchyID = 0;
                    }
                }
                rm.txtContent = "[textless]";
                rm.seqID = newSeqID;
                rm.branchID = newBranchID;
                rm.hierarchyID = newHierarchyID;
                rm.subpartID = newSubpartID;
                return rm;
            case ModuleManager.ModuleTypes.TICTACM:
                TicTacToe tttm = (TicTacToe)TicTacInspector.CreateModule(newAssetName + "-MG_TicTacToe");
                if (thisMod != null)
                {
                    tttm.previousModule = thisMod;
                    tttm.sendingCharacter = thisMod.sendingCharacter;
                    if (thisMod.previousModule == null)
                    {
                        thisMod.seqID = thisMod.branchID = thisMod.hierarchyID = 0;
                    }
                }
                tttm.txtContent = "[textless]";
                tttm.seqID = newSeqID;
                tttm.branchID = newBranchID;
                tttm.hierarchyID = newHierarchyID;
                tttm.subpartID = newSubpartID;
                return tttm;
            default:
                return null;
        }
    }

    public virtual void OnEnable()
    {
        mod = target as ModuleBlueprint;
        showSectionPrev = showSectionMsg = showSectionLog = showSectionNext = false;

        partPrevious = new GUIContent("Previous module");
        partMessage = new GUIContent("Message properties");
        partLog = new GUIContent("Log entry");
        partNext = new GUIContent("Next module");
        partDelete = new GUIContent("Delete");

        contentLabel = new GUIContent("Text content");
        prevMLabel = new GUIContent("Previous module", "is usually set automatically when using this Inspector's Create button");
        nextMLabel = new GUIContent("Next module asset", "is usually set automatically when using this Inspector's Create button");
        charLabel = new GUIContent("Character", "which character is sending this?");
        logLabel = new GUIContent("Log entry", "log");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //------------------------------------------------------
        reqStyle = new GUIStyle(GUI.skin.label);
        required = "(*)";
        reqStyle.normal.textColor = Color.red;
        reqStyle.fixedWidth = 20.0f;

        PartInfo();

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label(partPrevious, EditorStyles.boldLabel);
        showSectionPrev = EditorGUILayout.Foldout(showSectionPrev, showSectionPrev ? "Hide" : "Show");
        if (showSectionPrev)
        {
            PartPrevious();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label(partMessage, EditorStyles.boldLabel);
        showSectionMsg = EditorGUILayout.Foldout(showSectionMsg, showSectionMsg ? "(*) Hide" : "(*) Show");
        if (showSectionMsg)
        {
            PartMessage();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label(partLog, EditorStyles.boldLabel);
        showSectionLog = EditorGUILayout.Foldout(showSectionLog, showSectionLog ? "Hide" : "Show");
        if (showSectionLog)
        {
            PartLog();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label(partNext, EditorStyles.boldLabel);
        showSectionNext = EditorGUILayout.Foldout(showSectionNext, showSectionNext ? "Hide" : "Show");
        if (showSectionNext)
        {
            PartNext();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label(partDelete, EditorStyles.boldLabel);
        PartDelete();
        //------------------------------------------------------

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(mod);
    }

    public virtual void PartInfo()
    {
        GUILayout.Label("This is a " + mod.GetType().ToString(), EditorStyles.boldLabel);
        showHints = EditorGUILayout.Toggle("Show hints", showHints);
        EditorGUILayout.BeginHorizontal();
        if (!(showSectionPrev & showSectionMsg & showSectionLog & showSectionNext))
        {
            if (GUILayout.Button("Expand all", GUILayout.MaxWidth(100.0f)))
                showSectionPrev = showSectionMsg = showSectionLog = showSectionNext = true;
        }
        if ((showSectionPrev | showSectionMsg | showSectionLog | showSectionNext))
        {
            if (GUILayout.Button("Collapse all", GUILayout.MaxWidth(100.0f)))
                showSectionPrev = showSectionMsg = showSectionLog = showSectionNext = false;
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.Label("Fields labeled with (*) need to be set in order for the game to work.");

        if (mod.previousModule != null)
        {
            if (mod.previousModule.sendingCharacter == null)
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
    }
    public virtual void PartPrevious()
    {
        //previous module selection
        EditorGUILayout.BeginHorizontal();
        ModuleBlueprint prevMod = (ModuleBlueprint)EditorGUILayout.ObjectField(prevMLabel, mod.previousModule, typeof(ModuleBlueprint), false);
        if (prevMod != null)
        {
            mod.previousModule = prevMod;
            prevMod.nextModule = mod;
            fixID();
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
    public virtual void PartMessage() { }
    public virtual void PartLog()
    {
        if (showHints)
            EditorGUILayout.HelpBox("The log entry that will be added or updated when this module is triggered.", MessageType.Info);

        if (GUILayout.Button("Add Log Entry"))
        {
            mod.log = LogEntryInspector.CreateEntry(null);
        }
        mod.log = (LogEntry)EditorGUILayout.ObjectField(logLabel, mod.log, typeof(LogEntry), false);
    }
    public virtual void PartNext()
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
            Selection.activeObject = mod;
        }
    }
    public virtual void PartDelete()
    {
        if (showHints)
            EditorGUILayout.HelpBox("Permanently delete this module. This action cannot be reversed and is final.", MessageType.Info);

        GUIStyle bStyle = new GUIStyle(GUI.skin.button);
        bStyle.normal.textColor = Color.red;
        if (!reallyDelete && GUILayout.Button("Delete this module (irreversible)", bStyle))
        {
            reallyDelete = true;
        }

        if (reallyDelete)
        {
            EditorGUILayout.LabelField("Really delete?");

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Yes, really delete", bStyle))
            {
                reallyDelete = false;
                if (mod.previousModule != null)
                    mod.previousModule.nextModule = mod.nextModule;

                if (mod.nextModule != null)
                    mod.nextModule.previousModule = mod.previousModule;

                Debug.Log(mod.ToString() + " deleted? -> " + AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(mod)));
            }
            if (GUILayout.Button("No"))
            {
                reallyDelete = false;
            }
            EditorGUILayout.EndHorizontal();
        }
    }

    public virtual void fixID()
    {
        if (mod.seqID == -1 || mod.branchID == -1 || mod.hierarchyID == -1)
        {
            mod.seqID = mod.previousModule.seqID + 1;
            mod.branchID = mod.previousModule.branchID;
            mod.hierarchyID = mod.previousModule.hierarchyID;
        }
    }
}
