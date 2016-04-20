using UnityEngine;
using System.Collections;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class LogEntry : MonoBehaviour {

    private int logID = -1;
    private bool IDset = false;

    // Use this for initialization
    void Start()
    {
        if (!IDset)
        {
            logID = -1;
            Debug.LogError("logID not set for " + this.gameObject);
        }
    }

    public int GetModuleID()
    { return logID; }

    public bool SetModuleID(int id)
    {
        if (!IDset)
        {
            logID = id;
            IDset = true;
        }
        return IDset;
    }
}
