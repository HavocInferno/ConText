using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class ModuleManager : MonoBehaviour {

    /*modules dictionary receives all modules instanced at runtime, technically in order to handle hiding, access etc when needed, also to generally keep track.
    firstModule has to be the starting point of the story.
    nextModule internally temporarily stores a reference to the module next up for display.*/
    public Dictionary<int/*Pair<int,int>*/, GameObject> modules = new Dictionary<int/*Pair<int,int>*/, GameObject>();
    public ModuleBlueprint firstModule;
    private ModuleBlueprint nextModule;

    // Use this for initialization
	void Start () {
        nextModule = firstModule;
        //StartCoroutine(fireNext());
        goOn();
	}

    //rather self explanatory? Adds the given UI module/message instance plus the underlying Module's IDs to the dictionary
    public void addModuleToDict(int iID/*int modID, int subID*/, GameObject inst)
    {
        modules.Add(iID/*new Pair<int, int>(modID, subID)*/, inst);
    }

    /*coroutine to handle calling/firing of the module stream. checks whether text window is open, then triggers UI Manager to instance the next Module to be drawn.
    afterwards gets the next next Module down the line. technically does this as long as there is a nextModule returned by the last drawn one.
    ((here's hoping that putting a while loop into getNextPart - when a reply or game outcome is expected - actually stalls this one here...))*/
    IEnumerator fireNext()
    {
        Debug.Log("automatic text stream started");
        while (nextModule != null)
        {
            if (Unify.Instance.StateMng.GetGameState() == StateManager.GameState.TEXT)
            {
                yield return new WaitForSeconds(nextModule.delayBeforeSend);
                Unify.Instance.UIMng.addModule(nextModule);
                nextModule = nextModule.getNextPart();
            } else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
        Debug.Log("automatic text stream stopped. (due to module awaiting input? then that module should be starting the text stream again. otherwise...tough luck!)");
    }
    
    //this function serves for when the "window" is switched (and including the gameState) back to the Text view. needs to be called then in order for the text stream to continue.
    public void goOn()
    {
        StartCoroutine(fireNext());
    }

    /*this function serves mostly for modules that dont continue instantly but await input. 
    Then the fireNext coroutine will inevitably stop and for an unknown amount of time, nothing new might be fired. 
    The module in question would then, upon having computed its input, call goOnWith(x) with the respective followup module given as x.*/
    public void goOnWith(ModuleBlueprint mdl)
    {
        nextModule = mdl;
        StartCoroutine(fireNext());
    }
}
