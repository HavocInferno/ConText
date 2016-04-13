using UnityEngine;
using System.Collections;

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
    public InputManager InMng;
}
