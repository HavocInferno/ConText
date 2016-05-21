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
    }
}
