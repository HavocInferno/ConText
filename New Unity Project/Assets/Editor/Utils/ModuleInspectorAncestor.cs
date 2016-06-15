using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class ModuleInspectorAncestor : Editor { 

    public virtual ModuleBlueprint createNextModule(ModuleManager.ModuleTypes mtype, ModuleBlueprint thisMod)
    {
        switch(mtype)
        {
            case ModuleManager.ModuleTypes.TEXTM:
                thisMod.nextModule = TextModuleInspector.CreateModule();
                thisMod.nextModule.previousModule = thisMod;
                ((TextModule)thisMod.nextModule).txtContent = "[textless]";
                thisMod.nextModule.sendingCharacter = thisMod.sendingCharacter;
                if (thisMod.previousModule == null)
                {
                    thisMod.seqID = thisMod.branchID = thisMod.hierarchyID = 0;
                }
                thisMod.nextModule.seqID = thisMod.seqID + 1;
                thisMod.nextModule.branchID = thisMod.branchID;
                thisMod.nextModule.hierarchyID = thisMod.hierarchyID;
                return thisMod.nextModule;
            case ModuleManager.ModuleTypes.IMGM:
                return null;
            case ModuleManager.ModuleTypes.REPLYM:
                return null;
            case ModuleManager.ModuleTypes.TICTACM:
                return null;
            default:
                return null;
        }
    }
}
