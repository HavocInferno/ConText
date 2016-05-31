using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TicTacToe : TextModule {

    public ModuleBlueprint moduleSuccess, moduleFailure, moduleTie;

    public void triggerNext(bool humanWon, bool tie)
    {
        if (tie)
        {
            Unify.Instance.ModMng.goOnWith(moduleTie);
        }
        else
        {
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
}
