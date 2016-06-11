using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReplyButton : MonoBehaviour {

    public ReplyModule.ReplyOption option;
    public GameObject parentContainer;
    public ReplyModule parentModule;

    public void pressed()
    {
        parentModule.continueWithReply(option);
        //Unify.Instance.ModMng.goOnWith(outcome);
        foreach(Button b in parentContainer.GetComponentsInChildren<Button>())
        {
            b.interactable = false;
        }
    }
}
