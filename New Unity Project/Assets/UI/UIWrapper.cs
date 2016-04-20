using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class UIWrapper : MonoBehaviour {

    /*
    For starters, three layers. Specified as gameobjects for extensive handling.
    Vector2s for saving the positions and dimensions of the center and one of the off-center layers (requires Menu to be the centered layer on startup).
    */
    public GameObject MenuLayer;
    public GameObject TextLayer;
    public GameObject LogLayer;

    public Scrollbar vertScrollbar;

    private Vector2 anchorCenter, offsetCenter, anchorOffC, offsetOffC;
    private GameObject currentLayer;

	// Use this for initialization
	void Start () {
        //initially set the anchors and dimensions of center and off center recttransform info.
        anchorCenter = MenuLayer.GetComponent<RectTransform>().anchoredPosition;
        offsetCenter = MenuLayer.GetComponent<RectTransform>().anchoredPosition;
        anchorOffC = TextLayer.GetComponent<RectTransform>().anchoredPosition;
        offsetOffC = TextLayer.GetComponent<RectTransform>().anchoredPosition;

        //make sure Menu layer is loaded in right at the start
        currentLayer = MenuLayer;
        TextLayer.SetActive(false);
        LogLayer.SetActive(false);
        ToLayer(MenuLayer);

        Debug.Log(currentLayer.GetComponent<RectTransform>().anchoredPosition + ", " + currentLayer.GetComponent<RectTransform>().offsetMax); //offsetmax is negated though
    }

    /*first set the pos/size of the current layer to off-center (kinda redundant), then deactivate it.
    Then reverse order for the new layer.
    Then determine which layer was now cast in and tell the state manager about the new gamestate.*/
    public void ToLayer(GameObject lyr)
    {
        currentLayer.GetComponent<RectTransform>().anchoredPosition = anchorOffC;
        currentLayer.GetComponent<RectTransform>().offsetMax = offsetOffC;
        currentLayer.SetActive(false);

        currentLayer = lyr;
        currentLayer.SetActive(true);
        currentLayer.GetComponent<RectTransform>().anchoredPosition = anchorCenter;
        currentLayer.GetComponent<RectTransform>().offsetMax = offsetCenter;

        if (currentLayer == MenuLayer)
            Unify.Instance.StateMng.SetGameState(StateManager.GameState.MENU);
        else if (currentLayer == TextLayer)
            Unify.Instance.StateMng.SetGameState(StateManager.GameState.TEXT);
        else if (currentLayer == LogLayer)
            Unify.Instance.StateMng.SetGameState(StateManager.GameState.LOG);

        Debug.Log(Unify.Instance.StateMng.GetGameState());
    }

    public void scrollToZero()
    {
        vertScrollbar.value = 0f;
    }
}
