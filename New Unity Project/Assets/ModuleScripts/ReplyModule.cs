using UnityEngine;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class ReplyModule : TextModule {
    
    [System.Serializable]
    public class ReplyOption
    {
        public string replyText = "reply";
        public ModuleBlueprint outcome;
    }

    public List<ReplyOption> outcomes = new List<ReplyOption>();   
}
