using UnityEngine;
using UnityEditor;

public class ContextEditorWindow : EditorWindow
{
    string myString = "This is work in progress";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    [MenuItem("ConText/Project Overview")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        ContextEditorWindow window = (ContextEditorWindow)EditorWindow.GetWindow(typeof(ContextEditorWindow), false, "ConText Overview", true);
        window.Show();
    }

    void OnGUI()
    {
        /*GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();*/
        GUIStyle wrapStyle = new GUIStyle(GUI.skin.label);
        wrapStyle.wordWrap = true;

        GUILayout.Label("Welcome to your ConText Framework project.", EditorStyles.boldLabel);
        GUILayout.Label("This framework is intended to be used for creating choice based text adventures for mobile devices. It comes with all modules and tools necessary to create such a game in its basic form. Experienced users may expand the given modules with own code. Please refer to the documentation if you have troubles using this.", wrapStyle);

        if (Unify.Instance.UIMng.UISettings == null)
        {
            GUILayout.Label("No UI Settings specified yet.");
            if(GUILayout.Button("Add UI Settings"))
            {
                Unify.Instance.UIMng.UISettings = UISettingsInspector.CreateModule("UISettings_0");
            }
        } else
        {
            if(GUILayout.Button("Go to UI Settings"))
            {
                Selection.activeObject = Unify.Instance.UIMng.UISettings;
            }
        }

        if(Unify.Instance.ModMng.firstModule == null)
        {
            GUILayout.Label("No story specified yet.");
            if (GUILayout.Button("Create your story"))
            {
                ModuleManager.ModuleTypes nextModType = ModuleManager.ModuleTypes.TEXTM;
                nextModType = (ModuleManager.ModuleTypes)EditorGUILayout.Popup("Next module type", (int)nextModType, ModuleManager.m_ModuleTypeEnumDescriptions);

                Unify.Instance.ModMng.firstModule = ModuleInspectorAncestor.createNextModule(nextModType, null, 0, 0, 0, 0);
            }
        } else
        {
            if (GUILayout.Button("Go to story"))
            {
                Selection.activeObject = Unify.Instance.ModMng.firstModule;
            }
        }
    }
}