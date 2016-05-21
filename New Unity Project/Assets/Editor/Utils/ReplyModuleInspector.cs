﻿using UnityEngine;
using UnityEditor;
using System.Collections;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

//custom editor for Reply Modules.
[CustomEditor(typeof(ReplyModule))]
public class ReplyModuleInspector : TextModuleInspector {

    private ReplyModule rmod;
    private GUIContent repliesLabel = new GUIContent("Replies");

    [MenuItem("Assets/Create/ConText Framework/Modules/Reply Module")]
    public static ModuleBlueprint CreateModule()
    {
        return AssetCreator.CreateCustomAsset<ReplyModule>();
    }

    public void OnEnable()
    {
        base.OnEnable();

        rmod = target as ReplyModule;
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
            rmod.outcomes[i].outcome = (ModuleBlueprint)EditorGUILayout.ObjectField("Module", rmod.outcomes[i].outcome, typeof(ModuleBlueprint), false);
            EditorGUILayout.EndVertical();

            if (GUILayout.Button("Delete"))
            {
                rmod.outcomes.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add reply option"))
        {
            ReplyModule.ReplyOption r = new ReplyModule.ReplyOption();
            r.replyText = "new reply";
            r.outcome = TextModuleInspector.CreateModule();
            rmod.outcomes.Add(r);
            r.outcome.previousModule = rmod;
            if (rmod.previousModule == null)
            {
                rmod.moduleID = 0;
            }
            r.outcome.moduleID = rmod.moduleID;
            r.outcome.subID = rmod.outcomes.Count;
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(rmod);
    }

    public override void ModuleSpecific()
    {
        //
    }
}
