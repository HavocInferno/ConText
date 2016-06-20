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
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}