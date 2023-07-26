using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
//using XNode.Examples.StateGraph;

public class XNodeDecorator : XNodeBT {

	[Input] public Empty enter;
	[Output] public Empty exit;


	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	
	
	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
	{
		List<BTNode> children = GetNextBtNodes(bt);
		
		if (children == null || children.Count < 1)
		{
			Debug.Log("No children given for "+this.GetType().Name);
			return CreateBtNode(bt, null);
		}
		return CreateBtNode(bt, children[0]);
	}

	protected virtual BTNode CreateBtNode(BehaviourTree bt, BTNode child)
	{
		return new BTDecorator(bt, child);
	}


	protected override bool OnCheckForConfigChanges()
	{
		return false;
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