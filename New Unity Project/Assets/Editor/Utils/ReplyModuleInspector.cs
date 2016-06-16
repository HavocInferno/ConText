using UnityEngine;
using UnityEditor;
using System.Collections;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

//custom editor for Reply Modules.
[CustomEditor(typeof(ReplyModule))]
public class ReplyModuleInspector : TextModuleInspector {

    private ReplyModule rmod;
    //private ModuleManager.ModuleTypes nextModType;
    private GUIContent repliesLabel = new GUIContent("Replies");

    [MenuItem("Assets/Create/ConText Framework/Modules/Reply Module")]
    public static ModuleBlueprint CreateModule(string name)
    {
        return AssetCreator.CreateCustomAsset<ReplyModule>(name);
    }

    public void OnEnable()
    {
        base.OnEnable();

        rmod = target as ReplyModule;

        Debug.Log(rmod.ToString() + "; " + rmod.textHandover);
    }

    public override void drawTextField(int height)
    {
        base.drawTextField(200);
    }

    public override void OnInspectorGUI()
    {
        /*replaces default inspector with specifics for reply modules,
        i.e. display base stuff (from TextModule/ModuleBlueprint) as well as a list of replies with linked outcomes
        */

        base.OnInspectorGUI();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField(repliesLabel);
        for (int i = 0; i < rmod.outcomes.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical();
            rmod.outcomes[i].replyText = EditorGUILayout.TextArea(rmod.outcomes[i].replyText);
            EditorGUILayout.LabelField("choice ID: " + rmod.outcomes[i].choiceID);
            rmod.outcomes[i].outcome = (ModuleBlueprint)EditorGUILayout.ObjectField("Module", rmod.outcomes[i].outcome, typeof(ModuleBlueprint), false);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Delete"))
            {
                rmod.outcomes.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        nextModType = (ModuleManager.ModuleTypes)EditorGUILayout.Popup("Next module type", (int)nextModType, ModuleManager.m_ModuleTypeEnumDescriptions);
        if (GUILayout.Button("Add reply option (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
        {
            ReplyModule.ReplyOption r = new ReplyModule.ReplyOption();
            r.outcome = createNextModule(nextModType, rmod, 0, rmod.outcomes.Count, rmod.hierarchyID + 1, rmod.subpartID);

            if (rmod.outcomes.Count < 1)
            {
                r.choiceID = 0;
            }
            else
            {
                r.choiceID = rmod.outcomes[rmod.outcomes.Count - 1].choiceID + 1;
            }
            rmod.outcomes.Add(r);

            r.replyText = "new reply " + r.outcome.branchID;
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(rmod);
    }

    public override void ModuleSpecific()
    {
        //
    }
}
