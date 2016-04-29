using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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

    //public GameObject buttonCont;

    public List<ReplyOption> outcomes = new List<ReplyOption>();

    public override void setContent(GameObject UIObjectInstance)
    {
        base.setContent(UIObjectInstance);

        GameObject buttonCont = UIObjectInstance.GetComponentInChildren<ModuleUIHelper>().ButtonContainer;

        foreach(ReplyOption r in outcomes)
        {
            GameObject button = Instantiate(Unify.Instance.UIMng.UIWrap.ReplyButtonTemplate);
            button.transform.SetParent(buttonCont.transform);
            button.GetComponentInChildren<Text>().text = r.replyText;
            button.GetComponentInChildren<ReplyButton>().outcome = r.outcome;
            button.GetComponentInChildren<ReplyButton>().parentContainer = buttonCont;
        }
    }

    public override ModuleBlueprint getNextPart()
    {
        return null;
    }
}
