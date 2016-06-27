using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class ImageModule : ModuleBlueprint
{
    public Sprite imgContent;

    public override void setContent(GameObject UIObjectInstance)
    {
        base.setContent(UIObjectInstance);

        Debug.Log("setting content for ID " + seqID + "s-" + branchID + "b-" + hierarchyID + "h-" + subpartID + "sp");
        Image UIContent = null;
        if (UIContent == null)
        {
            UIContent = UIObjectInstance.GetComponentInChildren<ModuleUIHelper>().ImageContainer.GetComponentInChildren<Image>();
            if (UIContent == null)
            {
                Debug.LogError("this module's UI Object is missing an Image element; " + getUIObject().ToString());
            }
        }

        UIContent.sprite = imgContent;
        UIContent.color = Color.white;

        switch (sendingCharacter.alignment)
        {
            case Character.blobAlignment.LEFT:
                UIObjectInstance.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.right = 50;
                if (UIContent.GetComponentInChildren<LayoutElement>() != null)
                    UIContent.GetComponentInChildren<LayoutElement>().minWidth -= 50;
                break;
            case Character.blobAlignment.RIGHT:
                UIObjectInstance.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.left = 50;
                if (UIContent.GetComponentInChildren<LayoutElement>() != null)
                    UIContent.GetComponentInChildren<LayoutElement>().minWidth -= 50;
                break;
            default:
                UIObjectInstance.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.left = UIObjectInstance.GetComponent<HorizontalOrVerticalLayoutGroup>().padding.right = 0;
                break;
        }

        if (sendingCharacter.blobBackground != null)
        {
            Image tmp = UIObjectInstance.GetComponentInChildren<ModuleUIHelper>().TextContainer.GetComponentInChildren<Image>(); ;
            tmp.sprite = sendingCharacter.blobBackground;
            tmp.color = Color.white;
            tmp.enabled = true;
        }

        Unify.Instance.LogMng.fireLog(log);
    }

    public override ModuleBlueprint getNextPart()
    {
        pushChoice(null);

        return nextModule;
    }

    public override void pushChoice(IDChoiceCapsule idc)
    {
        idc = new IDChoiceCapsule();
        idc.SetModuleID(seqID, branchID, hierarchyID, subpartID);

        base.pushChoice(idc);
    }

    public override ModuleBlueprint getModForChoice(int choiceID, IDChoiceCapsule idc)
    {
        return nextModule;
    }
}
