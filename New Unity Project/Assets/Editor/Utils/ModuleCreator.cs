using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class ModuleCreator {

	public static T CreateModuleAsset<T> () where T : ScriptableObject
    {
        T mAsset = ScriptableObject.CreateInstance<T>();
        string aPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        if (aPath == "")
        {
            AssetDatabase.CreateFolder("Assets", "MiscModules");
            aPath = "Assets/MiscModules";
        } else if (Path.GetExtension(aPath) != "")
        {
            aPath = aPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        AssetDatabase.CreateAsset(mAsset,
            AssetDatabase.GenerateUniqueAssetPath(aPath + "/" + typeof(T).ToString() + ".asset"));
        AssetDatabase.SaveAssets();

        //EditorUtility.FocusProjectWindow();
        Selection.activeObject = mAsset;
        return mAsset;
    }
}
