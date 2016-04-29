using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Character))]
public class CharacterInspector : Editor
{

    private Character ch;

    void OnEnable()
    {
        ch = target as Character;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        EditorUtility.SetDirty(ch);
    }
}
