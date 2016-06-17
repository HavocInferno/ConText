﻿using UnityEngine;
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
                ReplyModule rm = (ReplyModule)ReplyModuleInspector.CreateModule(newAssetName + "-ReplyModule");
                rm.previousModule = thisMod;
                rm.txtContent = "[textless]";
                rm.sendingCharacter = thisMod.sendingCharacter;
                if (thisMod.previousModule == null)
                {
                    thisMod.seqID = thisMod.branchID = thisMod.hierarchyID = 0;
                }
                rm.seqID = newSeqID;
                rm.branchID = newBranchID;
                rm.hierarchyID = newHierarchyID;
                rm.subpartID = newSubpartID;
                return rm;
            case ModuleManager.ModuleTypes.TICTACM:
                TicTacToe tttm = (TicTacToe)TicTacInspector.CreateModule(newAssetName + "-ReplyModule");
                tttm.previousModule = thisMod;
                tttm.txtContent = "[textless]";
                tttm.sendingCharacter = thisMod.sendingCharacter;
                if (thisMod.previousModule == null)
                {
                    thisMod.seqID = thisMod.branchID = thisMod.hierarchyID = 0;
                }
                tttm.seqID = newSeqID;
                tttm.branchID = newBranchID;
                tttm.hierarchyID = newHierarchyID;
                tttm.subpartID = newSubpartID;
                return tttm;
            default:
                return null;
        }
    }
}
