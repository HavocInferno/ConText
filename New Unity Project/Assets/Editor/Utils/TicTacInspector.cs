using UnityEngine;
using UnityEditor;


/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(TicTacToe))]
public class TicTacInspector : ModuleInspectorAncestor
{
    private SerializedProperty textContent;
    private SerializedProperty successModule, failureModule, tieModule;
    private GUIContent succMLabel, failMLabel, tieMLabel;

    [MenuItem("Assets/Create/ConText Framework/Modules/Minigames/TicTacToe2")]
    public static ModuleBlueprint CreateModuleManual()
    {
        return AssetCreator.CreateCustomAsset<TicTacToe>(null);
    }

    public static ModuleBlueprint CreateModule(string name)
    {
        return AssetCreator.CreateCustomAsset<TicTacToe>(name);
    }

    public override void OnEnable()
    {
        base.OnEnable();

        mod = target as TicTacToe;

        textContent = serializedObject.FindProperty("txtContent");

        partNext = new GUIContent("Next modules (Win/Fail/Loss)");

        succMLabel = new GUIContent("Win module", "is usually set automatically when using this Inspector's Create button");
        failMLabel = new GUIContent("Lose module", "is usually set automatically when using this Inspector's Create button");
        tieMLabel = new GUIContent("Tie module", "is usually set automatically when using this Inspector's Create button");
    }

    public virtual void drawTextField(int height)
    {
        EditorGUILayout.PropertyField(textContent, contentLabel, GUILayout.MaxHeight(height));
    }

    public override void PartMessage()
    {
        EditorGUILayout.LabelField("message ID: " + mod.seqID + "(seq) " + mod.branchID + "(branch) " + mod.hierarchyID + "(hierarchy)");
        EditorGUILayout.Space();

        //character sending the message -> change to dropdown (enum of all characters, then determine forward/backward?)
        mod.sendingCharacter = (Character)EditorGUILayout.ObjectField(charLabel, mod.sendingCharacter, typeof(Character), false);
        mod.delayBeforeSend = EditorGUILayout.FloatField("Delay before sending (seconds)", mod.delayBeforeSend);

        //text input
        drawTextField(500);
    }
    public override void PartNext()
    {
        if (showHints)
            EditorGUILayout.HelpBox("The modules being triggered upon win/lose/tie game outcome.", MessageType.Info);

        nextModType = (ModuleManager.ModuleTypes)EditorGUILayout.Popup("Module type", (int)nextModType, ModuleManager.m_ModuleTypeEnumDescriptions);
        EditorGUILayout.Space();

        //success module selection
        EditorGUILayout.BeginHorizontal();
        ModuleBlueprint succMod = (ModuleBlueprint)EditorGUILayout.ObjectField(succMLabel, ((TicTacToe)mod).moduleSuccess, typeof(ModuleBlueprint), false);
        if (succMod != null)
        {
            ((TicTacToe)mod).moduleSuccess = succMod;
        }
        EditorGUILayout.LabelField(getShortDesc(succMod), GUILayout.MaxWidth(getShortDesc(succMod).Length * 7.0f));
        if (succMod == null)
        {
            if (GUILayout.Button("+ (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
            {
                ((TicTacToe)mod).moduleSuccess = createNextModule(nextModType, mod, 0, 0, mod.hierarchyID + 1, mod.subpartID);
            }
        }
        if (succMod != null)
        {
            if (GUILayout.Button("Go to"))
                Selection.activeObject = succMod;
        }
        EditorGUILayout.EndHorizontal();

        //failure module selection
        EditorGUILayout.BeginHorizontal();
        ModuleBlueprint failMod = (ModuleBlueprint)EditorGUILayout.ObjectField(failMLabel, ((TicTacToe)mod).moduleFailure, typeof(ModuleBlueprint), false);
        if (failMod != null)
        {
            ((TicTacToe)mod).moduleFailure = failMod;
        }
        EditorGUILayout.LabelField(getShortDesc(failMod), GUILayout.MaxWidth(getShortDesc(failMod).Length * 7.0f));
        if (failMod == null)
        {
            if (GUILayout.Button("+ (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
            {
                ((TicTacToe)mod).moduleFailure = createNextModule(nextModType, mod, 0, 1, mod.hierarchyID + 1, mod.subpartID);
            }
        }
        if (failMod != null)
        {
            if (GUILayout.Button("Go to"))
                Selection.activeObject = failMod;
        }
        EditorGUILayout.EndHorizontal();

        //tie module selection
        EditorGUILayout.BeginHorizontal();
        ModuleBlueprint tieMod = (ModuleBlueprint)EditorGUILayout.ObjectField(tieMLabel, ((TicTacToe)mod).moduleTie, typeof(ModuleBlueprint), false);
        if (tieMod != null)
        {
            ((TicTacToe)mod).moduleTie = tieMod;
        }
        EditorGUILayout.LabelField(getShortDesc(tieMod), GUILayout.MaxWidth(getShortDesc(tieMod).Length * 7.0f));
        if (tieMod == null)
        {
            if (GUILayout.Button("+ (" + ModuleManager.m_ModuleTypeEnumDescriptions[(int)nextModType] + ")"))
            {
                ((TicTacToe)mod).moduleTie = createNextModule(nextModType, mod, 0, 2, mod.hierarchyID + 1, mod.subpartID);
            }
        }
        if (tieMod != null)
        {
            if (GUILayout.Button("Go to"))
                Selection.activeObject = tieMod;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
    }
}
