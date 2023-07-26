using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[NodeEditor.CustomNodeEditorAttribute(typeof(XNodeBT))]
public class XNodeBTEditor : NodeEditor
{
    private XNodeBT node;
    private BTNodeGraphEditor graphEditor;

    public override void OnHeaderGUI() {
        // Initialization
        if (node == null) {
            node = target as XNodeBT;
            graphEditor = NodeGraphEditor.GetEditor(target.graph, window) as BTNodeGraphEditor;
        }

        base.OnHeaderGUI();
        Rect dotRect = GUILayoutUtility.GetLastRect();
        dotRect.size = new Vector2(16, 16);
        dotRect.y += 6;

        //graphEditor.GetLerpColor(Color.red, Color.green, node, node.led);
        GUI.color = Color.magenta;
        if (graphEditor.bt != null)
        {
            if (Application.isPlaying)
            {
                BTNode btNode = node.GetBtNode(graphEditor.bt);
                BTNode.Result result;
                if (btNode != null && btNode.Tree.GetStateValue(graphEditor.selectedInstanceID, btNode, out result))
                {
                    switch (result)
                    {
                        case BTNode.Result.Failure:
                            GUI.color = Color.red;
                            break;
                        case BTNode.Result.Running:
                            GUI.color = Color.yellow;
                            break;
                        case BTNode.Result.Success:
                            GUI.color = Color.green;
                            break;
                        case BTNode.Result.Inactive:
                            GUI.color = Color.gray;
                            break;
                    }
                }
                
            }
        }
        
        GUI.DrawTexture(dotRect, NodeEditorResources.dot);
        GUI.color = Color.white;
    }
}
