using UnityEngine;
using System.Collections;
using UnityEditor;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(LogEntry))]
public class LogEntryInspector : Editor {

    //private SerializedProperty modID;
    private SerializedProperty parentEntry;
    private SerializedProperty textContent;

    private LogEntry logE;
    private GUIContent textLabel, parentLabel, logIDLabel;

    [MenuItem("Assets/Create/ConText Framework/Log/Log Entry")]
    public static LogEntry CreateEntry(string name)
    {
        return AssetCreator.CreateCustomAsset<LogEntry>(name);
    }

    public void OnEnable()
    {
        //modID = serializedObject.FindProperty("moduleID");
        textContent = serializedObject.FindProperty("txtContent");

        logE = target as LogEntry;

        textLabel = new GUIContent("Log text");
        parentLabel = new GUIContent("Parent entry", "is usually set automatically when using this Inspector's Create button");
        logIDLabel = new GUIContent("log ID", "is usually set automatically when using this Inspector's Create button");
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
        LogEntry parentE = (LogEntry)EditorGUILayout.ObjectField(parentLabel, logE.parent, typeof(LogEntry), false);
        if (parentE != null || true)
        {
            logE.parent = parentE;
        }
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.BeginHorizontal();
        logE.logID = EditorGUILayout.IntField(logIDLabel, logE.logID);
        EditorGUILayout.EndHorizontal();

        //text input
        drawTextField(500);

        ModuleSpecific();

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(logE);
    }

    public virtual void ModuleSpecific()
    {
        //create a new module and give it a fitting set of IDs
        //{TODO}: selection list where the user can select which *type* of module is up next
        if (GUILayout.Button("Create next log"))
        {
            LogEntry newLog = CreateEntry(null);
            newLog.parent = logE;
            newLog.logID = logE.logID * 10 + (logE.children.Count + 1); //bad, temp, doesnt allow for more than 9 children without issues
            newLog.txtContent = "Prev log entry: " + logE.logID;

            logE.children.Add(newLog);
        }
    }
}
