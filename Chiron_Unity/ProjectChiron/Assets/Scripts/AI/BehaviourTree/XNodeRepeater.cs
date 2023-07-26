using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class XNodeRepeater : XNodeDecorator
{

	public int RepeatXTimes;
	
	protected override void Init() {
		base.Init();
		
	}
	
	private void Reset()
	{
		name = "Repeater";
	}

	protected override BTNode CreateBtNode(BehaviourTree bt, BTNode child)
	{
		btNode = new BTRepeater(bt, child);
		OnWriteValuesToBTNode();
		return btNode;
	}

	protected override bool CheckForNodeChanges()
	{
		return false;
	}
	
	//this is called from OnValidate in base class 
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
			return;
		}
	}

	protected override void OnWriteConfigValues()
	{
		if(entity==null)
			return;
	}

	public override void OnWriteValuesToBTNode()
	{
		BTRepeater btRepeater = btNode as BTRepeater;
		btRepeater.RepeatXTimes = RepeatXTimes;
	}
	
}