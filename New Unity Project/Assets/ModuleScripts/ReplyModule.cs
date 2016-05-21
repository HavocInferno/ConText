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

    public List<ReplyOption> outcomes = new List<ReplyOption>();

    public override void setContent(GameObject UIObjectInstance)
    {
        base.setContent(UIObjectInstance);

        GameObject buttonCont = UIObjectInstance.GetComponentInChildren<ModuleUIHelper>().ButtonContainer;

        /*since the module itself is basically just a text module with replies, do the base function, 
        then simply attach all possible replies*/
        foreach(ReplyOption r in outcomes)
        {
            GameObject button = Instantiate(Unify.Instance.UIMng.UIWrap.ReplyButtonTemplate);
            button.transform.SetParent(buttonCont.transform);
            button.GetComponentInChildren<Text>().text = r.replyText;
            button.GetComponentInChildren<ReplyButton>().outcome = r.outcome;
            button.GetComponentInChildren<ReplyButton>().parentContainer = buttonCont;
        }
    }

    /*this module is essentially Push for progression, not Pull, 
    so return null here in order to halt automatic module loading. 
    Note: in order for things to work out then, the buttons need to start module loading again. 
    The ReplyButton class does that for its use.*/
    public override ModuleBlueprint getNextPart()
    {
        return null;
    }
}
