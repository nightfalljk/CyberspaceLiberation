using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[NodeWidth(304)]
public class XNodeParallel : XNodeComposite
{

	public BTNode.ResultSetting ResultSetting = BTNode.ResultSetting.AllSuccessOrOneRunning;
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}
	
	private void Reset()
	{
		name = "Parallel";
	}
	
	public override BTNode CreateBtNodes(BehaviourTree bt, List<BTNode> children)
	{
		btNode = new BTParallel(bt, children.ToArray());
		OnWriteValuesToBTNode();
		return btNode;
	}

	public override void OnWriteValuesToBTNode()
	{
		BTParallel btParallel = btNode as BTParallel;
		btParallel._ResultSetting = ResultSetting;
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