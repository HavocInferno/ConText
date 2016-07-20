using UnityEngine; //*
using UnityEditor; //*
using System.Collections.Generic;

/*Lines marked with '//*' as taken from https://community.unity.com/t5/Extensions-OnGUI/Simple-node-editor/td-p/1448640 */

public class NodeViewer : EditorWindow //*
{ //*
    public class GraphNode
    {
        public Rect window;
        public ModuleBlueprint mod;
        public List<int> neighbors;
        public bool _nodeOption, _handleActive;
    }

    public Rect _handleArea;
    private bool _options, _action;
    private Texture2D _resizeHandle, _aaLine;
    private GUIContent _icon;
    private int _mainwindowID;
    private int i = 0;
    GUIStyle wrapStyle;

    Dictionary<int, GraphNode> idNodes;
    List<GraphNode> nodes;
    float iw_x, iw_y, iw_width, iw_height;
    float w_x, w_y, w_width, w_height;

    [MenuItem("ConText/Story graph")] //*
    static void ShowEditor() //*
    { //*
        NodeViewer editor = (NodeViewer)EditorWindow.GetWindow(typeof(NodeViewer), false, "ConText Graph", true); //*
        editor.ShowNodes(); //Init(); //*
    } //*

    private void ShowNodes()
    {
        i = 0;

        w_x = iw_x = 10f;
        w_y = iw_y = 30f;
        w_width = iw_width = 250f;
        w_height = iw_height = 400f;

        _resizeHandle = AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeViewer/Icons/ResizeHandle.png", typeof(Texture2D)) as Texture2D;
        //_aaLine = AssetDatabase.LoadAssetAtPath("Assets/Editor/NodeViewer/Icons/AA1x5.png", typeof(Texture2D)) as Texture2D;
        _icon = new GUIContent(_resizeHandle);
        _mainwindowID = GUIUtility.GetControlID(FocusType.Native); //grab primary editor window controlID

        nodes = new List<GraphNode>();
        idNodes = new Dictionary<int, GraphNode>();
        ModuleBlueprint nextmod = Unify.Instance.ModMng.firstModule;

        resetCFNrec(nextmod);
        addGN(nextmod, -1, w_x, w_y);
        /*foreach(GraphNode gn in nodes)
        {
            gn.mod.checkedForNode = false;
        }*/
    }
    
    private void resetCFNrec(ModuleBlueprint mod)
    {
        mod.checkedForNode = false;
        mod.nodeID = -1;

        foreach(ModuleBlueprint mb in mod.getAllNext())
        {
            if (mb != null)
            {
                if (mb.checkedForNode)
                    resetCFNrec(mb);
            }
        }
    }
    private void addGN(ModuleBlueprint mod, int prevNodeID, float x, float y)
    {
        Debug.Log("checking " + mod.ToString() + "; cFN " + mod.checkedForNode);
        int tmpID;
        if (!mod.checkedForNode)
        {
            GraphNode gn = new GraphNode();
            gn.window = new Rect(x, y, w_width, w_height);
            gn.mod = mod;
            gn.neighbors = new List<int>();
            if (prevNodeID > -1)
            {
                gn.neighbors.Add(prevNodeID);
            }
            mod.nodeID = i;
            mod.checkedForNode = true;

            nodes.Add(gn);
            idNodes.Add(i, gn);

            tmpID = i;
            i++;
        } else
        {
            tmpID = mod.nodeID;
        }

        //here, do next
        ModuleBlueprint[] allNext = mod.getAllNext();
        int j = 0;
        foreach (ModuleBlueprint mb in allNext)
        {
            if (mb != null)
            {
                addGN(mb, tmpID, x + iw_width + 3 * w_x, w_y + j * (iw_height + w_y));
                j++;
            }
        }
    }

    void OnGUI() //*
    { //*
        wrapStyle = new GUIStyle(GUI.skin.label);
        wrapStyle.wordWrap = true;

        BeginWindows(); //*
        if (idNodes != null)
        {
            foreach (KeyValuePair<int, GraphNode> kvp in idNodes)
            {
                GraphNode gn = kvp.Value;
                gn.window = GUI.Window(kvp.Key, gn.window, DrawNodeWindow, gn.mod.name);

                if (gn.neighbors != null)
                {
                    foreach (int no in gn.neighbors)
                    {
                        DrawNodeCurve(idNodes[no].window, gn.window);
                    }
                }
            }
        }
        EndWindows(); //*

        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        _options = GUILayout.Toggle(_options, "Toggle Me", EditorStyles.toolbarButton);
        if(GUILayout.Button("Reset layout", EditorStyles.toolbarButton))
        {
            ShowNodes();
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        //if drag extends inner window bounds _handleActive remains true as event gets lost to parent window
        if ((Event.current.rawType == EventType.MouseUp) && (GUIUtility.hotControl != _mainwindowID))
        {
            GUIUtility.hotControl = 0;
        }
    } //*

    void DrawNodeWindow(int id) //*
    { //*
        if (GUIUtility.hotControl == 0)  //mouseup event outside parent window?
        {
            idNodes[id]._handleActive = false; //make sure handle is deactivated
        }

        float _cornerX = 0f;
        float _cornerY = 0f;
        _cornerX = idNodes[id].window.width;
        _cornerY = idNodes[id].window.height;

        //begin layout of contents
        GUILayout.BeginArea(new Rect(1, 16, _cornerX - 3, _cornerY - 1));
        GUILayout.BeginHorizontal(EditorStyles.toolbar);
        idNodes[id]._nodeOption = GUILayout.Toggle(idNodes[id]._nodeOption, "show debug", EditorStyles.toolbarButton);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        if (idNodes[id]._nodeOption)
        {
            GUILayout.Label("x: " + idNodes[id].window.x + " y: " + idNodes[id].window.y);
            GUILayout.Label("parents:");
            foreach (int no in idNodes[id].neighbors)
            {
                GUILayout.Label(no.ToString() + " (" + idNodes[no].mod.ToString() + ")");
            }
        }
        if (GUILayout.Button("Go to"))
        {
            Selection.activeObject = idNodes[id].mod;
        }
        ModuleBlueprint.NodeContent mnc = idNodes[id].mod.getContentForNode();
        if (mnc != null)
        {
            if (mnc.minigame)
            {
                GUILayout.Label("This is a " + mnc.minigameName + " minigame module.", wrapStyle);
            }
            if(mnc.ch != null)
            {
                GUILayout.Label("Character: " + mnc.ch.characterName, wrapStyle);
            }
            GUILayout.Label("Content:");
            if (mnc.text != null)
            {
                GUILayout.Label(mnc.text, wrapStyle);
            }
            if (mnc.img != null)
            {
                EditorGUILayout.ObjectField(mnc.img, typeof(Sprite), false, GUILayout.MaxHeight(idNodes[id].window.width));
            }
        } else
        {
            GUILayout.Label("Error: Couldn't fetch module content.", wrapStyle);
        }


        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(1, _cornerY - 16, _cornerX - 3, 14));
        GUILayout.BeginHorizontal(EditorStyles.toolbarTextField, GUILayout.ExpandWidth(true));
        GUILayout.FlexibleSpace();

        //grab corner area based on content reference
        _handleArea = GUILayoutUtility.GetRect(_icon, GUIStyle.none);
        GUI.DrawTexture(new Rect(_handleArea.xMin + 6, _handleArea.yMin - 3, 20, 20), _resizeHandle); //hacky placement
        _action = (Event.current.type == EventType.MouseDown) || (Event.current.type == EventType.MouseDrag);
        if (!idNodes[id]._handleActive && _action)
        {
            if (_handleArea.Contains(Event.current.mousePosition, true))
            {
                idNodes[id]._handleActive = true; //active when cursor is in contact area
                GUIUtility.hotControl = GUIUtility.GetControlID(FocusType.Native); //set handle hot
            }
        }

        EditorGUIUtility.AddCursorRect(_handleArea, MouseCursor.ResizeUpLeft);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        //resize window
        if (idNodes[id]._handleActive && (Event.current.type == EventType.MouseDrag))
        {
            ResizeNode(id, Event.current.delta.x, Event.current.delta.y);
            Repaint();
            Event.current.Use();
        }

        //enable drag for node
        if (!idNodes[id]._handleActive)
        {
            GUI.DragWindow();
        }
    } //*

    private void ResizeNode(int id, float deltaX, float deltaY)
    {
        float _w = idNodes[id].window.width;
        float _h = idNodes[id].window.height;

        if ((_w + deltaX) > iw_width) { idNodes[id].window.xMax += deltaX; }
        if ((_h + deltaY) > iw_height) { idNodes[id].window.yMax += deltaY; }
    }

    void DrawNodeCurve(Rect start, Rect end) //*
    { //*
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0); //*
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0); //*
        Vector3 startTan = startPos + Vector3.right * 50; //*
        Vector3 endTan = endPos + Vector3.left * 50; //*
        Color shadowCol = new Color(0, 0, 0, 0.06f); //*
        for (int i = 0; i < 3; i++) // Draw a shadow //*
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5); //*
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1); //*
    } //*
} //*