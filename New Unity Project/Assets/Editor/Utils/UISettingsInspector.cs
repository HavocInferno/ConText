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
    private GUIContent bg_ModColorLabel, bg_ModFontLabel, bg_ModFontSizeLabel, bg_ModImageLabel;
    private GUIContent bg_ViewsColorLabel, bg_ViewsFontLabel, bg_ViewsFontSizeLabel, bg_ViewsImageLabel;
    private SerializedProperty modTemplates;
    private bool showHints = false;

    [MenuItem("Assets/Create/ConText Framework/Misc/UI Settings (don't add multiple)")]
    public static UISettings CreateModule(string name)
    {
        return AssetCreator.CreateCustomAsset<UISettings>(name);
    }

    public void OnEnable()
    {
        modTemplates = serializedObject.FindProperty("moduleTemplates");

        uis = target as UISettings;

        bg_ModColorLabel = new GUIContent("Mod Bg Clr");
        bg_ModFontLabel = new GUIContent("Module/message font");
        bg_ModFontSizeLabel = new GUIContent("Module/message font Size");
        bg_ModImageLabel = new GUIContent("Mod Image");

        bg_ViewsColorLabel = new GUIContent("Views background color");
        bg_ViewsFontLabel = new GUIContent("Views font");
        bg_ViewsFontSizeLabel = new GUIContent("Views font Size");
        bg_ViewsImageLabel = new GUIContent("Views Image");

        uis.MenuView = GameObject.FindGameObjectWithTag("UIDummy").GetComponent<UIWrapper>().MenuLayer;
        uis.TextView = GameObject.FindGameObjectWithTag("UIDummy").GetComponent<UIWrapper>().TextLayer;
        uis.LogView = GameObject.FindGameObjectWithTag("UIDummy").GetComponent<UIWrapper>().LogLayer;

        uis.MenuImage = uis.MenuView.GetComponentInChildren<Image>();
        uis.TextImage = uis.TextView.GetComponentInChildren<Image>();
        uis.LogImage = uis.LogView.GetComponentInChildren<Image>();

        uis.viewTextFont = uis.MenuView.GetComponentInChildren<Text>().font;
        uis.viewTextFontSize = uis.MenuView.GetComponentInChildren<Text>().fontSize;
        uis.viewBackgroundColor = uis.MenuView.GetComponentInChildren<Image>().color;
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
        showHints = EditorGUILayout.Toggle("Show hints", showHints);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (showHints)
            EditorGUILayout.HelpBox("All existing character info is grabbed from the Story Settings asset.", MessageType.Info);
        uis.sSettings = (StorySettings)EditorGUILayout.ObjectField("Story settings asset" ,uis.sSettings, typeof(StorySettings), false);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (showHints)
            EditorGUILayout.HelpBox("Color and background image for the respective character's messages.", MessageType.Info);
        if (uis.sSettings != null)
        {
            foreach(Character ch in uis.sSettings.characters)
            {
                ch.blobColor = EditorGUILayout.ColorField("[" + ch.name + "] message color", ch.blobColor);
                ch.blobBackground = (Sprite)EditorGUILayout.ObjectField("[" + ch.name + "] msg background image", ch.blobBackground, typeof(Sprite), false);
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        /*for settings related to module properties*/
        #region ModuleRelatedFields
        if (showHints)
            EditorGUILayout.HelpBox("Add all your module UI templates here. This list is used to apply the below properties.", MessageType.Info);
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.PropertyField(modTemplates, true);
        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();

        EditorGUILayout.BeginVertical();
        //
        EditorGUILayout.BeginHorizontal();
        uis.moduleTextFont = (Font)EditorGUILayout.ObjectField(bg_ModFontLabel, uis.moduleTextFont, typeof(Font), false);
        EditorGUILayout.EndHorizontal();
        //
        EditorGUILayout.BeginHorizontal();
        uis.moduleTextFontSize = EditorGUILayout.IntField(bg_ModFontSizeLabel, uis.moduleTextFontSize);
        EditorGUILayout.EndHorizontal();
        //
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        #endregion

        /*for settings related to view properties (views being Menu, Text, Log etc)*/
        #region ViewRelatedFields
        if (showHints)
            EditorGUILayout.HelpBox("'Views' are the Menu, the Text stream and the Log list. These settings affect all three the same.", MessageType.Info);

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
        EditorGUILayout.BeginHorizontal();
        int viewSize = EditorGUILayout.IntField(bg_ViewsFontSizeLabel, uis.viewTextFontSize);
        uis.viewTextFontSize = viewSize;

        if (GUILayout.Button("Apply"))
        {
            foreach (Text ia in uis.MenuView.GetComponentsInChildren<Text>())
            {
                ia.fontSize = viewSize;
            }
            foreach (Text ia in uis.TextView.GetComponentsInChildren<Text>())
            {
                ia.fontSize = viewSize;
            }
            foreach (Text ia in uis.LogView.GetComponentsInChildren<Text>())
            {
                ia.fontSize = viewSize;
            }
        }
        EditorGUILayout.EndHorizontal();
        //
        uis.MenuImage.sprite = (Sprite)EditorGUILayout.ObjectField("Menu background image", uis.MenuImage.sprite, typeof(Sprite), false);
        uis.TextImage.sprite = (Sprite)EditorGUILayout.ObjectField("Text background image", uis.TextImage.sprite, typeof(Sprite), false);
        uis.LogImage.sprite = (Sprite)EditorGUILayout.ObjectField("Log background image", uis.LogImage.sprite, typeof(Sprite), false);

        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        #endregion

        /*for configuring which modules use which templates*/
        #region ModTemplatesStuff
        if (showHints)
            EditorGUILayout.HelpBox("This list pairs the respective modules to their UI templates. Put the class name of a new module into the left text field of an entry, and the corresponding UI template into the right. \n\nDo not change this unless you know what you are doing!", MessageType.Info);
        EditorGUI.BeginChangeCheck();

        int i = 1;
        foreach (UISettings.modUIPair mup in uis.modUITemplates)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField(i + ".", GUILayout.MaxWidth(14.0f));
            mup.modClassName = EditorGUILayout.TextField(mup.modClassName);
            mup.modUITemplate = (GameObject)EditorGUILayout.ObjectField(mup.modUITemplate, typeof(GameObject), false);

            if (GUILayout.Button("X"))
            {
                uis.modUITemplates.Remove(mup);
            }

            EditorGUILayout.EndHorizontal();
            i++;
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

