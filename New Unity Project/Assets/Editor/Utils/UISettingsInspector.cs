using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

[CustomEditor(typeof(UISettings))]
public class UISettingsInspector : Editor {

    //private SerializedProperty modID;

    private UISettings uis;
    private GUIContent bg_ModColorLabel, bg_ModFontLabel, bg_ViewsColorLabel, bg_ViewsFontLabel;
    private SerializedProperty modTemplates;

    [MenuItem("Assets/Create/ConText Framework/Misc/UI Settings (don't add multiple)")]
    public static UISettings CreateModule()
    {
        return AssetCreator.CreateCustomAsset<UISettings>();
    }

    public void OnEnable()
    {
        modTemplates = serializedObject.FindProperty("moduleTemplates");

        uis = target as UISettings;

        bg_ModColorLabel = new GUIContent("Mod Bg Clr");
        bg_ModFontLabel = new GUIContent("Mod Font");

        bg_ViewsColorLabel = new GUIContent("Views Bg Clr");
        bg_ViewsFontLabel = new GUIContent("Views Font");

        uis.MenuView = GameObject.FindGameObjectWithTag("UIDummy").GetComponent<UIWrapper>().MenuLayer;
        uis.TextView = GameObject.FindGameObjectWithTag("UIDummy").GetComponent<UIWrapper>().TextLayer;
        uis.LogView = GameObject.FindGameObjectWithTag("UIDummy").GetComponent<UIWrapper>().LogLayer;
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        serializedObject.Update();

        serializedObject.ApplyModifiedProperties();

        ModuleSpecific();

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(uis);
    }

    public virtual void ModuleSpecific()
    {
        uis.sSettings = (StorySettings)EditorGUILayout.ObjectField(uis.sSettings, typeof(StorySettings), false);

        if(uis.sSettings != null)
        {
            foreach(Character ch in uis.sSettings.characters)
            {
                ch.blobColor = EditorGUILayout.ColorField(ch.name + " blob color", ch.blobColor);
            }
        }

        /*for settings related to module properties*/
        #region ModuleRelatedFields
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(modTemplates, true);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        EditorGUILayout.BeginVertical();
        //
        EditorGUILayout.BeginHorizontal();
        Font bgFont = (Font)EditorGUILayout.ObjectField(bg_ModFontLabel, uis.moduleTextFont, typeof(Font), false);
        uis.moduleTextFont = bgFont;

        if (GUILayout.Button("Apply"))
        {
            foreach (GameObject a in uis.moduleTemplates)
            {
                foreach (Text ia in a.GetComponentsInChildren<Text>())
                {
                    ia.font = bgFont;
                }
            }
        }
        EditorGUILayout.EndHorizontal();
        //
        EditorGUILayout.EndVertical();
        #endregion

        /*for settings related to view properties (views being Menu, Text, Log etc)*/
        #region ViewRelatedFields
        EditorGUILayout.BeginVertical();
        //
        EditorGUILayout.BeginHorizontal();
        Color viewCol = EditorGUILayout.ColorField(bg_ViewsColorLabel, uis.viewBackgroundColor);
        uis.viewBackgroundColor = viewCol;

        if (GUILayout.Button("Apply"))
        {
            foreach (Image ia in uis.MenuView.GetComponentsInChildren<Image>())
            {
                ia.color = viewCol;
            }
            foreach (Image ia in uis.TextView.GetComponentsInChildren<Image>())
            {
                ia.color = viewCol;
            }
            foreach (Image ia in uis.LogView.GetComponentsInChildren<Image>())
            {
                ia.color = viewCol;
            }
        }

        EditorGUILayout.EndHorizontal();
        //
        EditorGUILayout.BeginHorizontal();
        Font viewFont = (Font)EditorGUILayout.ObjectField(bg_ViewsFontLabel, uis.viewTextFont, typeof(Font), false);
        uis.viewTextFont = viewFont;

        if (GUILayout.Button("Apply"))
        {
            foreach (Text ia in uis.MenuView.GetComponentsInChildren<Text>())
            {
                ia.font = viewFont;
            }
            foreach (Text ia in uis.TextView.GetComponentsInChildren<Text>())
            {
                ia.font = viewFont;
            }
            foreach (Text ia in uis.LogView.GetComponentsInChildren<Text>())
            {
                ia.font = viewFont;
            }
        }
        EditorGUILayout.EndHorizontal();
        //
        EditorGUILayout.EndVertical();
        #endregion

        /*for configuring which modules use which templates*/
        #region ModTemplatesStuff
        EditorGUI.BeginChangeCheck();
        
        foreach (UISettings.modUIPair mup in uis.modUITemplates)
        {
            EditorGUILayout.BeginHorizontal();

            mup.modClassName = EditorGUILayout.TextField(mup.modClassName);
            mup.modUITemplate = (GameObject)EditorGUILayout.ObjectField(mup.modUITemplate, typeof(GameObject), false);

            if (GUILayout.Button("X"))
            {
                uis.modUITemplates.Remove(mup);
            }

            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add template pair"))
        {
            UISettings.modUIPair mp = new UISettings.modUIPair();
            mp.modClassName = "Empty";
            mp.modUITemplate = null;
            uis.modUITemplates.Add(mp);
        }

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
        #endregion
    }
}

