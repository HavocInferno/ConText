using UnityEngine;
using UnityEditor;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

//custom editor for Reply Modules.
[CustomEditor(typeof(ReplyModule))]
public class ReplyModuleInspector : TextModuleInspector {

    private ReplyModule rmod;
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

    public override void OnEnable()
    {
        base.OnEnable();

        rmod = target as ReplyModule;

        for(int i = 0; i < rmod.outcomes.Count; i++)
        {
            if(rmod.outcomes[i].outcome == null)
            {
                rmod.outcomes.RemoveAt(i);
            }
        }

        partNext = new GUIContent("Replies");

        Debug.Log(rmod.ToString() + "; " + rmod.textHandover);
    }

    public override void drawTextField(int height)
    {
        base.drawTextField(200);
    }

    public override void PartNext()
    {
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
            Selection.activeObject = rmod;

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
    }
    public override void PartDelete()
    {
        if (showHints)
            EditorGUILayout.HelpBox("Permanently delete this module. This action cannot be reversed and is final.", MessageType.Info);

        GUIStyle bStyle = new GUIStyle(GUI.skin.button);
        bStyle.normal.textColor = Color.red;
        if (GUILayout.Button("Delete this module (irreversible)", bStyle))
        {
            if (rmod.previousModule != null)
                rmod.previousModule.nextModule = rmod.outcomes[0].outcome;

            if (rmod.outcomes[0].outcome != null)
                rmod.outcomes[0].outcome.previousModule = rmod.previousModule;

            Debug.Log(rmod.ToString() + " deleted? -> " + AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rmod)));
        }
    }
}
