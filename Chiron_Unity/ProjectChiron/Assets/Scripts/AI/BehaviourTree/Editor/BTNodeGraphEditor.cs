using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(XNodeGraph))]
public class BTNodeGraphEditor : NodeGraphEditor
{
    public BehaviourTree bt;
    public int selectedInstanceID;
    //private int lastSelectedInstanceID;
    
    public override void OnGUI()
    {
        //base.OnGUI();
        window.Repaint();
        
        XNodeGraph nodeGraph = target as XNodeGraph;
        bt = nodeGraph.bt;
        //if (nodeGraph.root != null)
        {
            //nodeGraph.selectedInstanceID = nodeGraph.selectedInstanceID;
            selectedInstanceID = nodeGraph.GetSelectedInstanceId();
        }

//        if (lastSelectedInstanceID != selectedInstanceID)
//        {
//            foreach (XNodeBT node in nodeGraph.nodes)
//            {
//                node.On
//            }
//            lastSelectedInstanceID = selectedInstanceID;
//        }
        //selectedInstanceID = bt.ro
    }

    public override void OnDropObjects(Object[] objects)
    {
        base.OnDropObjects(objects);
    }
}
