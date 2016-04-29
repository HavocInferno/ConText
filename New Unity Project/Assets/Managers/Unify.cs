using UnityEngine;
using System.Collections;

/*--------------------------------
Copyright 2016 - Paul Preißner - for Bachelor Thesis "ConText - A Choice/Text Adventure Framework" @ TU München
--------------------------------*/

/*intended to simply unify public access to globally needed/requested stuff like references to the managers
essentially a fancier name for classic GameData script*/
public class Unify : MonoBehaviour {

    private static Unify instance;

    private Unify()
    {
        if (instance != null)
            return;

        instance = this;
    }

    public static Unify Instance
    {
        get
        {
            if (instance == null)
                new Unify();

            return instance;
        }
    }

    public UIManager UIMng;
    public ModuleManager ModMng;
    public StateManager StateMng;
    public LogManager LogMng;
}
