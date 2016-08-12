using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class ReplyButton : MonoBehaviour {

    public ReplyModule.ReplyOption option;
    public GameObject parentContainer;
    public ReplyModule parentModule;

    public void pressed()
    {
        parentModule.continueWithReply(option);
        foreach(Button b in parentContainer.GetComponentsInChildren<Button>())
        {
            b.interactable = false;
        }
    }
}
