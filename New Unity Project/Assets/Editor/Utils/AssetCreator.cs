using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class AssetCreator {

    //creates a new asset of type T
	public static T CreateCustomAsset<T> () where T : ScriptableObject
    {
        T mAsset = ScriptableObject.CreateInstance<T>();
        string aPath = AssetDatabase.GetAssetPath(Selection.activeObject);

        //puts all created assets, where a location isn't specified, into a specific folder for unsorted items
        if (aPath == "")
        {
            AssetDatabase.CreateFolder("Assets", "Unsorted Misc");
            aPath = "Assets/Unsorted Misc";
        } else if (Path.GetExtension(aPath) != "")
        {
            aPath = aPath.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
        }

        //there a way to give it more clever default names? maybe give the types/classes static functions for something better?
        AssetDatabase.CreateAsset(mAsset,
            AssetDatabase.GenerateUniqueAssetPath(aPath + "/New" + typeof(T).ToString() + ".asset"));
        AssetDatabase.SaveAssets();

        Selection.activeObject = mAsset;
        return mAsset;
    }
}
