using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class AssetCreator {

    //creates a new asset of type T
	public static T CreateCustomAsset<T> (string name) where T : ScriptableObject
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
        Debug.Log(name);
        string assetName = aPath + "/New" + typeof(T).ToString() + ".asset";
        if (name != null)
        {
            if (!name.Equals(""))
                assetName = aPath + "/" + name + ".asset";
        } 
        AssetDatabase.CreateAsset(mAsset,
            AssetDatabase.GenerateUniqueAssetPath(assetName));
        AssetDatabase.SaveAssets();

        Selection.activeObject = mAsset;
        return mAsset;
    }
}
