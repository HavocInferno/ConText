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
        public int choiceID = -1;
        public ModuleBlueprint outcome;
    }

    public List<ReplyOption> outcomes = new List<ReplyOption>();
    public int chosen = -1;
    public bool textHandover = true;

    public override void setContent(GameObject UIObjectInstance)
    {
        Debug.Log("this should be 1st + " + textHandover);
        if (textHandover)
        {
            Debug.Log("first texthandover, destroying bogus reply module instance.");
            Destroy(UIObjectInstance);
        } else {
            Debug.Log("texthandover done, proceed with normal setContent.");
            base.setContent(UIObjectInstance);

            GameObject buttonCont = UIObjectInstance.GetComponentInChildren<ModuleUIHelper>().ButtonContainer;

            //temp fix. need to decouple text part and button part of it or something, since as it stands, the default padding code in TextModule leads to skewed padding layout with other modules when ReplyModule modules are used
            UIObjectInstance.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.left = UIObjectInstance.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.right = 0;

            /*since the module itself is basically just a text module with replies, do the base function, 
            then simply attach all possible replies*/
            foreach (ReplyOption r in outcomes)
            {
                GameObject button = Instantiate(Unify.Instance.UIMng.UIWrap.ReplyButtonTemplate);
                button.transform.SetParent(buttonCont.transform);
                button.GetComponentInChildren<Text>().text = r.replyText;
                button.GetComponentInChildren<ReplyButton>().option = r;
                button.GetComponentInChildren<ReplyButton>().parentContainer = buttonCont;
                button.GetComponentInChildren<ReplyButton>().parentModule = this;
                if (Unify.Instance.StateMng.initialLoad)
                {
                    foreach (Button b in button.GetComponentsInChildren<Button>())
                    {
                        b.interactable = false;
                    }
                }
            }

            //if(Unify.Instance.StateMng.initialLoad)
             //   textHandover = true;
        }
    }

    /*this module is essentially Push for progression, not Pull, 
    so return null here in order to halt automatic module loading. 
    Note: in order for things to work out then, the buttons need to start module loading again. 
    The ReplyButton class does that for its use.*/
    public override ModuleBlueprint getNextPart()
    {
        Debug.Log("this should be 2nd + " + textHandover);
        if (textHandover)
        {
            Debug.Log("handing over reply module -> text module");
            TextModule tm = CreateInstance<TextModule>();
            tm.delayBeforeSend = delayBeforeSend;
            tm.sendingCharacter = sendingCharacter;
            tm.previousModule = previousModule;
            tm.nextModule = this;
            tm.txtContent = txtContent;
            tm.seqID = seqID; tm.branchID = branchID; tm.hierarchyID = hierarchyID; tm.subpartID = subpartID + 1;
            tm.UIObjectTemplate = UIObjectTemplate;

            textHandover = false;

            return tm;
        }
        else
        {
            textHandover = true;
            return null;
        }
    }

    public void continueWithReply(ReplyOption ro)
    {
        textHandover = true;
        chosen = ro.choiceID;
        pushChoice(null);
        Unify.Instance.ModMng.goOnWith(ro.outcome);
    }

    public override void pushChoice(IDChoiceCapsule idc)
    {
        idc = new IDChoiceCapsule();
        idc.SetModuleID(seqID, branchID, hierarchyID, subpartID);
        idc.choice = chosen;

        if (!Unify.Instance.StateMng.initialLoad)
            Unify.Instance.ModMng.addChoiceToList(this, idc);
        else
            Debug.Log("Not pushing due to initial load");
    }

    public override ModuleBlueprint getModForChoice(int choiceID)
    {
        if (textHandover)
        {
            Debug.Log("(getModForChoice) handing over reply module -> text module");
            TextModule tm = CreateInstance<TextModule>();
            tm.delayBeforeSend = delayBeforeSend;
            tm.sendingCharacter = sendingCharacter;
            tm.previousModule = previousModule;
            tm.nextModule = this;
            tm.txtContent = txtContent;
            tm.seqID = seqID; tm.branchID = branchID; tm.hierarchyID = hierarchyID; tm.subpartID = subpartID + 1;
            tm.UIObjectTemplate = UIObjectTemplate;

            textHandover = false;

            return tm;
        }
        else
        {
            textHandover = true;
            foreach (ReplyOption r in outcomes)
            {
                if (r.choiceID == choiceID)
                {
                    return r.outcome;
                }
            }
        }

        return nextModule;
    }
}
