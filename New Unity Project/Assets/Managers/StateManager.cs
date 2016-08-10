using UnityEngine;
//using UnityEditor;
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

    [System.Serializable]
    public class GameSettings
    {
        public bool mute = false;
        public float volume = 1.0f;
        public float soundMultiplier = 1.0f;

        public GameSettings()
        {
            mute = false;
            volume = soundMultiplier = 1.0f;
        }
    }
    public static GameSettings gameSettings = new GameSettings();

    private GameState gameState = GameState.MENU;
    public bool initialLoad = true;
    public static bool saveExists = false;

    public void Awake()
    {
        LoadSettings();
    }

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

        saveExists = true;
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
                Debug.Log("loaded entry " + i + ": [" + Unify.Instance.ModMng.choices[i].ToString() + "]--[" + Unify.Instance.ModMng.choices[i].choice + "]c");
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

    public static void deleteSaveFile(string fn)
    {
        string path = Application.persistentDataPath + "/Saves/" + fn + ".ctxt";
        File.Delete(path);
        saveExists = false;
    }

    public static void checkForFile(string fn)
    {
        string path = Application.persistentDataPath + "/Saves/" + fn + ".ctxt";
        saveExists = File.Exists(path);
    }

    public void deleteSaveIngame(string fn)
    {
        deleteSaveFile(fn);
        Unify.Instance.ModMng.resetStream(true);
    }

    public static void SaveSettings()
    {
        string path = Application.persistentDataPath + "/Config/config.ctxt";
        Debug.Log("Attempting config save to " + path);

        if (!File.Exists(path))
        {
            Debug.Log("First save? Creating directory.");
            Directory.CreateDirectory(Application.persistentDataPath + "/Config/");
        }

        BinaryFormatter bform = new BinaryFormatter();
        FileStream saveFile = File.Create(path);
        bform.Serialize(saveFile, gameSettings);
        Debug.Log("Likely config saved to " + path);

        saveFile.Close();
    }

    public static bool LoadSettings()
    {
        string path = Application.persistentDataPath + "/Config/config.ctxt";
        Debug.Log("Attempting config load from " + path);

        if (File.Exists(path))
        {
            BinaryFormatter bform = new BinaryFormatter();
            FileStream saveFile = File.Open(path, FileMode.Open);
            gameSettings = (GameSettings)bform.Deserialize(saveFile);
            Unify.Instance.UIMng.UIWrap.initSettings();

            Debug.Log("Likely config loaded from " + path);

            saveFile.Close();
            return true;
        }

        Debug.Log("couldn't load config (either not found or loading procedure failed)");
        return false;
    }
}
