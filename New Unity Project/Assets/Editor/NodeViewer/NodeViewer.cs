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
        public List<GraphNode> neighbors;
    }

    Rect window1; //*
    Rect window2; //*

    Dictionary<int, GraphNode> idNodes;
    List<GraphNode> nodes;
    float iw_x, iw_y, iw_width, iw_height;
    float w_x, w_y, w_width, w_height;

    [MenuItem("ConText/Story graph")] //*
    static void ShowEditor() //*
    { //*
        NodeViewer editor = (NodeViewer)EditorWindow.GetWindow(typeof(NodeViewer), false, "CT Story Graph", true); //*
        editor.Init(); //*
    } //*

    public void Init() //*
    { //*
        float w_x = iw_x = 10;
        float w_y = iw_y = 10;
        float w_width = iw_width = 150;
        float w_height = iw_height = 100;

        nodes = new List<GraphNode>();
        idNodes = new Dictionary<int, GraphNode>();
        ModuleBlueprint nextmod = Unify.Instance.ModMng.firstModule;

        int i = 0;
        while(nextmod != null)
        {
            GraphNode gn = new GraphNode();
            gn.window = new Rect(w_x, w_y, w_width, w_height);
            gn.mod = nextmod;

            nodes.Add(gn);
            idNodes.Add(i, gn);

            w_x += 2 * w_width + 10;
            nextmod = nextmod.nextModule;
            i++;
        }

        //window1 = new Rect(10, 10, 100, 100); //*
        //window2 = new Rect(210, 210, 100, 100); //*
    } //*

    void OnGUI() //*
    { //*
        //DrawNodeCurve(window1, window2); // Here the curve is drawn under the windows //*

        BeginWindows(); //*
        if (idNodes != null)
        {
            foreach (KeyValuePair<int, GraphNode> kvp in idNodes)
            {
                GraphNode gn = kvp.Value;
                GUI.Window(kvp.Key, gn.window, DrawNodeWindow, gn.mod.name);

                if (gn.neighbors != null)
                {
                    foreach (GraphNode neigh in gn.neighbors)
                    {
                        DrawNodeCurve(gn.window, neigh.window);
                    }
                }
            }
        }
        //window1 = GUI.Window(1, window1, DrawNodeWindow, "Window 1");   // Updates the Rect's when these are dragged //*
        //window2 = GUI.Window(2, window2, DrawNodeWindow, "Window 2"); //*
        EndWindows(); //*
    } //*

    void DrawNodeWindow(int id) //*
    { //*
        GUI.DragWindow(); //*
        if(GUI.Button(new Rect(5, 15, 50, 20), "Go to"))
        {
            GraphNode gn;
            Debug.Log("pipi");
            if (idNodes.TryGetValue(id, out gn)) {
                Selection.activeObject = gn.mod;
            }
        }
    } //*

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