using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class XNodeLeaf : XNodeBT {

	[Input] public Empty enter;
	// Use this for initialization
	protected override void Init() {
		base.Init();
		
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

	protected override bool CheckForNodeChanges()
	{
		return base.CheckForNodeChanges();
	}
}