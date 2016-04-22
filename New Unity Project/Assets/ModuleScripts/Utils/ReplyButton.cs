using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReplyButton : MonoBehaviour {

    public ModuleBlueprint outcome;
    public GameObject parentContainer;

    public void pressed()
    {
        Unify.Instance.ModMng.goOnWith(outcome);
        foreach(Button b in parentContainer.GetComponentsInChildren<Button>())
        {
            b.interactable = false;
        }
    }
}
