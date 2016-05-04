using UnityEngine;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class LogManager : MonoBehaviour {

    public Dictionary<int, GameObject> logEntries = new Dictionary<int, GameObject>();

    public void fireLog(LogEntry log)
    {
        if (log == null)
            return;

        Unify.Instance.UIMng.addLogEntry(log);
    }

    public void addLogToDict(int lID, GameObject logInst)
    {
        logEntries.Add(lID, logInst);
    }
}
