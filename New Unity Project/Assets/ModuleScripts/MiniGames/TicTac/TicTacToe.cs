using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TicTacToe : TextModule {

    public ModuleBlueprint moduleSuccess, moduleFailure, moduleTie;
    public bool playerWon, tied;

    public void triggerNext(bool humanWon, bool tie)
    {
        if (tie)
        {
            tied = tie;
            playerWon = false;
            pushChoice(null);
            Unify.Instance.ModMng.goOnWith(moduleTie);
        }
        else
        {
            tied = false;
            playerWon = humanWon;
            pushChoice(null);
            Unify.Instance.ModMng.goOnWith((humanWon ? moduleSuccess : moduleFailure));
        }
    }

    public override void setContent(GameObject UIObjectInstance)
    {
        base.setContent(UIObjectInstance);

        UIObjectInstance.GetComponentInChildren<TicTacBehavior>().baseModule = this;

        //temp fix. need to decouple text part and game part of it or something, since as it stands, the default padding code in TextModule leads to skewed padding layout with other modules when TicTacToe modules are used
        UIObjectInstance.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.left = UIObjectInstance.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.right = 0;
    }

    public override void pushChoice(IDChoiceCapsule idc)
    {
        idc = new IDChoiceCapsule();
        idc.SetModuleID(seqID, branchID, hierarchyID, subpartID);
        if (!tied && playerWon)
            idc.choice = 0;
        else if (!tied && !playerWon)
            idc.choice = 1;
        else if (tied)
            idc.choice = 2;
        else
            idc.choice = -1;

        if (!Unify.Instance.StateMng.initialLoad)
            Unify.Instance.ModMng.addChoiceToList(this, idc);
        else
            Debug.Log("Not pushing due to initial load");
    }

    public override ModuleBlueprint getModForChoice(int choiceID)
    {
        switch(choiceID)
        {
            case 0:
                return moduleSuccess;
            case 1:
                return moduleFailure;
            case 2:
                return moduleTie;
            default:
                return null;
        }
    }
}
