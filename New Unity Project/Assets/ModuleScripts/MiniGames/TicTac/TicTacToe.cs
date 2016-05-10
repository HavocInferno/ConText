using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TicTacToe : TextModule {

    public ModuleBlueprint moduleSuccess, moduleFailure;

    public void triggerNext(bool humanWon)
    {
        Unify.Instance.ModMng.goOnWith((humanWon ? moduleSuccess : moduleFailure));
    }

    public override void setContent(GameObject UIObjectInstance)
    {
        base.setContent(UIObjectInstance);

        UIObjectInstance.GetComponentInChildren<TicTacBehavior>().baseModule = this;
    }
}
