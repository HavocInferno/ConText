using UnityEngine;
using UnityEditor;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class ContextEditorWindow : EditorWindow
{
    bool groupEnabled;
    bool showHints = false;
    float buttonWidth = 300f;
    bool changeFirstMod = false;

    Vector2 scrollPos;

    [MenuItem("ConText/Project Options")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ContextEditorWindow window = (ContextEditorWindow)EditorWindow.GetWindow(typeof(ContextEditorWindow), false, "ConText Options", true);
        window.Show();
    }

    void OnEnable()
    {
        StateManager.checkForFile("mainStory");
        Debug.Log("beep");
    }

    void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos,
                                                      false,
                                                      false);

        if(Unify.Instance.ModMng == null || Unify.Instance.UIMng == null || Unify.Instance.StateMng == null || Unify.Instance.LogMng == null)
        {
            GUIStyle bStyle = new GUIStyle(GUI.skin.label);
            bStyle.normal.textColor = Color.red;
            bStyle.wordWrap = true;
            bStyle.fontSize = 20;
            GUILayout.Label("(Some) manager instances missing. Is Main.unity open?", bStyle);
            EditorGUILayout.EndScrollView();
            return;
        }

        GUIStyle wrapStyle = new GUIStyle(GUI.skin.label);
        wrapStyle.wordWrap = true;
        wrapStyle.fontStyle = FontStyle.Bold;
        int tmp = wrapStyle.fontSize;

        GUIStyle descStyle = new GUIStyle(GUI.skin.label);
        descStyle.wordWrap = true;
        descStyle.fontStyle = FontStyle.Bold;
        int tmp2 = descStyle.fontSize;

        GUILayout.Label("Welcome to your ConText Framework project.", wrapStyle);
        wrapStyle.fontStyle = FontStyle.Normal;
        GUILayout.Label("This framework is intended to be used for creating choice based text adventures for mobile devices. It comes with all modules and tools necessary to create such a game in its basic form. Experienced users may expand the given modules with own code. Please refer to the documentation if you have troubles using this.", wrapStyle);

        wrapStyle.fontSize = 10;

        showHints = EditorGUILayout.Toggle("Show hints", showHints);

        GUILayout.Label("--------------------------------------------------------------------------------------------------------------------------------", EditorStyles.centeredGreyMiniLabel);
        GUILayout.Label("Game settings", descStyle);
        if (Unify.Instance.UIMng.UISettings == null)
        {
            GUILayout.Label("No UI Settings specified yet.");
            if(GUILayout.Button("Add UI Settings", GUILayout.MaxWidth(buttonWidth)))
            {
                Unify.Instance.UIMng.UISettings = UISettingsInspector.CreateModule("UISettings_0");
            }
        } else
        {
            if(GUILayout.Button("Go to Game UI Settings", GUILayout.MaxWidth(buttonWidth)))
            {
                Selection.activeObject = Unify.Instance.UIMng.UISettings;
            }
            if(showHints)
                GUILayout.Label("Game UI settings encompass visual properties of the game screens as well as of the message modules for each character (color, font, background images).", wrapStyle);

            if (Unify.Instance.UIMng.UISettings.sSettings == null)
            {
                GUILayout.Label("No Character List specified yet.");
                if (GUILayout.Button("Add 'Character & Story' settings", GUILayout.MaxWidth(buttonWidth)))
                {
                    Unify.Instance.UIMng.UISettings.sSettings = StorySettingsInspector.Create();
                }
            }
            else
            {
                if (GUILayout.Button("Go to 'Character & Story' settings", GUILayout.MaxWidth(buttonWidth)))
                {
                    Selection.activeObject = Unify.Instance.UIMng.UISettings.sSettings;
                }
            }
        }
        if (GUILayout.Button("Go to 'Player Settings'", GUILayout.MaxWidth(buttonWidth)))
        {
            EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
        }
        if (showHints)
            GUILayout.Label("Player settings include settings such as the game's name, icon, etc. as well as the iOS/Android app settings.", wrapStyle);

        GUILayout.Label("--------------------------------------------------------------------------------------------------------------------------------", EditorStyles.centeredGreyMiniLabel);
        GUILayout.Label("Story", descStyle);
        if (Unify.Instance.ModMng.firstModule == null)
        {
            GUILayout.Label("No story specified yet.");
            if (GUILayout.Button("Create your story", GUILayout.MaxWidth(buttonWidth)))
            {
                ModuleManager.ModuleTypes nextModType = ModuleManager.ModuleTypes.TEXTM;
                nextModType = (ModuleManager.ModuleTypes)EditorGUILayout.Popup("Next module type", (int)nextModType, ModuleManager.m_ModuleTypeEnumDescriptions);

                Unify.Instance.ModMng.firstModule = ModuleInspectorAncestor.createNextModule(nextModType, null, 0, 0, 0, 0);
            }
        } else
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Go to first story module", GUILayout.MaxWidth(0.5f * buttonWidth)))
            {
                Selection.activeObject = Unify.Instance.ModMng.firstModule;
                Debug.Log(((ModuleBlueprint)Selection.activeObject).hierarchyID + " " + ((ModuleBlueprint)Selection.activeObject).branchID + " " + ((ModuleBlueprint)Selection.activeObject).seqID);
            }
            if (!changeFirstMod && GUILayout.Button("Swap first module", GUILayout.MaxWidth(0.5f * buttonWidth)))
            {
                changeFirstMod = true;
            }
            if (changeFirstMod)
            {
                Unify.Instance.ModMng.firstModule = (ModuleBlueprint)EditorGUILayout.ObjectField(Unify.Instance.ModMng.firstModule, typeof(ModuleBlueprint), false, GUILayout.MaxWidth(0.5f * buttonWidth));
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Go to latest story module", GUILayout.MaxWidth(buttonWidth)))
            {
                Selection.activeObject = Unify.Instance.ModMng.firstModule.getHighestModule();
                Debug.Log(((ModuleBlueprint)Selection.activeObject).hierarchyID + " " + ((ModuleBlueprint)Selection.activeObject).branchID + " " + ((ModuleBlueprint)Selection.activeObject).seqID);
            }
            Color tmpc = wrapStyle.normal.textColor;
            wrapStyle.normal.textColor = Color.red;
            if (showHints)
                GUILayout.Label("This is still experimental and might not return the correct module. Make sure to manually check.", wrapStyle);
            wrapStyle.normal.textColor = tmpc;

            GUILayout.Space(10);
            if (GUILayout.Button("Fix IDs (experimental)", GUILayout.MaxWidth(buttonWidth)))
            {
                Unify.Instance.ModMng.fixIDs();
            }
            if (showHints)
            {
                Color tmpc3 = wrapStyle.normal.textColor;
                wrapStyle.normal.textColor = Color.red;
                GUILayout.Label("This is still experimental and might break module IDs. Be aware.", wrapStyle);
                wrapStyle.normal.textColor = tmpc3;
            }
        }
        GUI.enabled = StateManager.saveExists;
        if (GUILayout.Button("Reset save file" + (StateManager.saveExists ? "" : " (no file found)"), GUILayout.MaxWidth(buttonWidth)))
        {
            StateManager.deleteSaveFile("mainStory");
            StateManager.checkForFile("mainStory");
        }
        GUI.enabled = true;
        if (showHints)
            GUILayout.Label("If your story changes, the save file will not fit anymore. This will delete the old file (if there is one).", wrapStyle);

        wrapStyle.fontSize = tmp;
        EditorGUILayout.EndScrollView();
    }
}