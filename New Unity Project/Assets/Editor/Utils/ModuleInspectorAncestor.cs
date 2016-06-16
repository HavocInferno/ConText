using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

public class ModuleInspectorAncestor : Editor { 

    public virtual ModuleBlueprint createNextModule(ModuleManager.ModuleTypes mtype, ModuleBlueprint thisMod, int newSeqID, int newBranchID, int newHierarchyID, int newSubpartID)
    {
        string newAssetName = "H" + newHierarchyID + ".B" + newBranchID + ".S" + newSeqID;
        switch (mtype)
        {
            case ModuleManager.ModuleTypes.TEXTM:
                TextModule tm = (TextModule)TextModuleInspector.CreateModule(newAssetName + "-TextModule");
                tm.previousModule = thisMod;
                tm.txtContent = "[textless]";
                tm.sendingCharacter = thisMod.sendingCharacter;
                if (thisMod.previousModule == null)
                {
                    thisMod.seqID = thisMod.branchID = thisMod.hierarchyID = 0;
                }
                tm.seqID = newSeqID;
                tm.branchID = newBranchID;
                tm.hierarchyID = newHierarchyID;
                tm.subpartID = newSubpartID;
                return tm;
            case ModuleManager.ModuleTypes.IMGM:
                ImageModule im = (ImageModule)ImageModuleInspector.CreateModule(newAssetName + "-ImageModule");
                im.previousModule = thisMod;
                im.sendingCharacter = thisMod.sendingCharacter;
                if (thisMod.previousModule == null)
                {
                    thisMod.seqID = thisMod.branchID = thisMod.hierarchyID = 0;
                }
                im.seqID = newSeqID;
                im.branchID = newBranchID;
                im.hierarchyID = newHierarchyID;
                im.subpartID = newSubpartID;
                return im;
            case ModuleManager.ModuleTypes.REPLYM:
                return null;
            case ModuleManager.ModuleTypes.TICTACM:
                return null;
            default:
                return null;
        }
    }
}
