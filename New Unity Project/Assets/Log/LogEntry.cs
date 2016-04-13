using UnityEngine;
using System.Collections;

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

    // Update is called once per frame
    void Update()
    {

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
