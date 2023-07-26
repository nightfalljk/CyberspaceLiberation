using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class XNodeComposite : XNodeBT {

	[Input] public Empty enter;
	[Output] public Empty exit;//change to multiple?
	
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
			return null;
		}
		//children.
		return CreateBtNodes(bt, children);
	}

	public virtual BTNode CreateBtNodes(BehaviourTree bt, List<BTNode> children)
	{
		return new BTComposite(bt, children.ToArray());
	}
	
	protected override bool OnCheckForConfigChanges()
	{
		bool change = false;
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