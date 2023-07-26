using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XNodeSimpleAction : XNodeLeaf
{
//    public override BTNode CreateBtNodes(BehaviourTree bt, List<BTNode> children)
//    {
//        return new BTSequencer(bt, children.ToArray());
//    }

    public string E_ActionName;
    private string lastActionName;
        
    protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        btNode = new BTSimpleAction(bt);
        OnWriteValuesToBTNode();
        return btNode;
    }

    private void Reset()
    {
        name = "SimpleAction";
    }

    public override void OnWriteValuesToBTNode()
    {
        BTSimpleAction btSimpleAction = btNode as BTSimpleAction;
        btSimpleAction.ActionName = E_ActionName;
    }

    protected override bool OnCheckForConfigChanges()
    {
        bool change = false;
        if (lastActionName != E_ActionName)
        {
            lastActionName = E_ActionName;
            change = true;
//            if (btNode != null)
//            {
//                
//            }
//            else
//            {
//                
//            }
        }
        return change;
    }
	
    public override void OnLoadEntity(Entity entity)
    {
        base.OnLoadEntity(entity);
        if (entity == null)
        {
            //SetDefault
            return;
        }
        //Set values from Config
    }
	
    protected override void OnWriteConfigValues()
    {
    }
}
