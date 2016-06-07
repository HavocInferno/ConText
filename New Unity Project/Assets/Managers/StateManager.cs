using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class StateManager : MonoBehaviour {

    public enum GameState
    {
        MENU,
        TEXT,
        LOG,
    }

    private GameState gameState = GameState.MENU;
    public bool initialLoad = true;

    public GameState GetGameState()
    { return gameState; }

    public void SetGameState(GameState gs)
    {
        gameState = gs;
    }

    public static void SaveChoices(string fileName)
    {
        string path = Application.persistentDataPath + "/Saves/" + fileName + ".ctxt";
        Debug.Log("Attempting file save to " + path);
        
        if(!File.Exists(path))
        {
            Debug.Log("First save? Creating directory.");
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/");
        }

        BinaryFormatter bform = new BinaryFormatter();
        FileStream saveFile = File.Create(path);
        bform.Serialize(saveFile, Unify.Instance.ModMng.choices);
        Debug.Log("Likely file saved to " + path);

        saveFile.Close();
    }

    public static bool LoadChoices(string fileName)
    {
        Unify.Instance.ModMng.choices.Clear();

        string path = Application.persistentDataPath + "/Saves/" + fileName + ".ctxt";
        Debug.Log("Attempting file load from " + path);

        if (File.Exists(path))
        {
            BinaryFormatter bform = new BinaryFormatter();
            FileStream saveFile = File.Open(path, FileMode.Open);
            Unify.Instance.ModMng.choices = (List<ModuleBlueprint.IDChoiceCapsule>)bform.Deserialize(saveFile);
            Debug.Log("Likely file loaded from " + path);

            for(int i = 0; i < Unify.Instance.ModMng.choices.Count; i++)
            {
                Debug.Log("loaded entry " + i + ": " + Unify.Instance.ModMng.choices[i].ToString());
            }

            saveFile.Close();
            return true;
        }

        Debug.Log("couldn't load file (either not found or loading procedure failed)");
        return false;
    }

    public static void wipeSaveFile(string fn)
    {
        Unify.Instance.ModMng.choices.Clear();
        SaveChoices(fn);
        Debug.Log("save file wiped");
    }
}
