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
    public static ModuleBlueprint CreateModuleManual()
    {
        return AssetCreator.CreateCustomAsset<ReplyModule>(null);
    }

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

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label(repliesLabel, EditorStyles.boldLabel);
        if (showHints)
            EditorGUILayout.HelpBox("The respective replies this module will offer to the player, i.e. the text for each option and the module to trigger.", MessageType.Info);

        for (int i = 0; i < rmod.outcomes.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("choice ID: " + rmod.outcomes[i].choiceID);
            if (GUILayout.Button("Delete"))
            {
                rmod.outcomes.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();

            rmod.outcomes[i].replyText = EditorGUILayout.TextArea(rmod.outcomes[i].replyText);

            EditorGUILayout.BeginHorizontal();
            rmod.outcomes[i].outcome = (ModuleBlueprint)EditorGUILayout.ObjectField("Module", rmod.outcomes[i].outcome, typeof(ModuleBlueprint), false);
            EditorGUILayout.LabelField(getShortDesc(rmod.outcomes[i].outcome), GUILayout.MaxWidth(getShortDesc(rmod.outcomes[i].outcome).Length * 10.0f));
            if (rmod.outcomes[i].outcome != null)
            {
                if (GUILayout.Button("Go to"))
                    Selection.activeObject = rmod.outcomes[i].outcome;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField("--------------------------------------------------------------------------------------------------------------------------------", GUILayout.MaxWidth(0.75f * EditorGUIUtility.currentViewWidth));
        }

        GUILayout.Label("Add reply", EditorStyles.boldLabel);
        nextModType = (ModuleManager.ModuleTypes)EditorGUILayout.Popup("Reply type", (int)nextModType, ModuleManager.m_ModuleTypeEnumDescriptions);
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
