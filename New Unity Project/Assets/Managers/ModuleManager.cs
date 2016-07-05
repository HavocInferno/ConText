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
    public List<ModuleBlueprint.IDChoiceCapsule> choices = new List<ModuleBlueprint.IDChoiceCapsule>();

    public Dictionary<string, GameObject> UITemplateMapping = new Dictionary<string, GameObject>();
    public ModuleBlueprint firstModule;
    private ModuleBlueprint nextModule;

    public enum ModuleTypes
    {
        TEXTM,
        IMGM,
        REPLYM,
        TICTACM
    }
    public static string[] m_ModuleTypeEnumDescriptions = new string[]
    {
        "Text Module",
        "Image Module",
        "Reply Module",
        "Tic Tac Toe Module"
    };

    // Use this for initialization
    void Start () {
        StateManager.LoadChoices("mainStory");

        Unify.Instance.UIMng.menuLayerSetStartButton(choices.Count > 0 ? "Continue" : "Start");

        foreach(UISettings.modUIPair mup in Unify.Instance.UIMng.UISettings.modUITemplates)
        {
            UITemplateMapping.Add(mup.modClassName, mup.modUITemplate);
        }

        nextModule = advanceToLatest();//firstModule;
        //StartCoroutine(fireNext());
        goOn();
	}

    //rather self explanatory? Adds the given UI module/message instance to the dictionary
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
                Unify.Instance.UIMng.UIWrap.typingIndicator.SetActive(true);
                yield return new WaitForSeconds(nextModule.delayBeforeSend);
                Unify.Instance.UIMng.addModule(nextModule);
                Unify.Instance.UIMng.UIWrap.typingIndicator.SetActive(false);
                nextModule = nextModule.getNextPart();
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }
        Debug.Log("automatic text stream stopped. (due to module awaiting input? then that module should be starting the text stream again. otherwise...tough luck!)");
    }

    public void fireInvidivual(ModuleBlueprint mod)
    {
        //if (Unify.Instance.StateMng.GetGameState() == StateManager.GameState.TEXT)
        //{
            Debug.Log("Firing individual mod " + mod.ToString());
            Unify.Instance.UIMng.addModule(mod);
        //}
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

    public bool addChoiceToList(ModuleBlueprint mod, ModuleBlueprint.IDChoiceCapsule id_choice)
    {
        /*Checks whether mod and choice capsule are same data, 
        then adds the capsule to the choices list (i.e. this function to be 
        called from within the specific module's code, as those have better 
        control over what choice ID to use etc)*/
        if (mod == null || id_choice == null)
            { Debug.Log("choice addition failed"); return false; }

        if (id_choice.checkIDequal(mod))
        {
            choices.Add(id_choice);
            StateManager.SaveChoices("mainStory");
            return true;
        }
        return false;
    }

    public ModuleBlueprint advanceToLatest()
    {
        Debug.Log("Attempting to load existing story progress.");
        ModuleBlueprint nextMod = firstModule;
        foreach (ModuleBlueprint.IDChoiceCapsule idc in choices)
        {
            if(idc.checkIDequal(nextMod))
            {
                fireInvidivual(nextMod);
                nextMod = nextMod.getModForChoice(idc.choice, idc);
                if (nextMod == null)
                    { Debug.Log("Couldn't load further story progress. No next module found. Actual end of story?"); return null; }
            } else
            {
                Debug.Log("Save file corrupt; does not fit with storyline.");
                Unify.Instance.StateMng.initialLoad = false;
                return firstModule; //not ideal, if corrupt, reset to 0?
            }
        }
        Unify.Instance.StateMng.initialLoad = false;

        return nextMod;
    }

    public void fixIDs()
    {
        firstModule.SetModuleID(0, 0, 0);
        firstModule.fixNextIDs();
    }
}
