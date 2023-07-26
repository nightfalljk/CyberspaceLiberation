using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class XNodeBtRandomWalk : XNodeLeaf
{
	//public BTRandomWalk BtRandomWalk;
	public BTRandomWalk.WalkType WalkType = BTRandomWalk.WalkType.SimpleSelf;
	public float MinDistance;
	public float MaxDistance;
	
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	private void Reset()
	{
		name = "RandomWalk";
	}

	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
	{
		btNode = new BTRandomWalk(bt);
		OnWriteValuesToBTNode();
		return btNode;
	}

	public override void OnWriteValuesToBTNode()
	{
		BTRandomWalk bTRandomWalk = btNode as BTRandomWalk;
		bTRandomWalk.walkType = WalkType;
		bTRandomWalk.minDistance = MinDistance;
		bTRandomWalk.maxDistance = MaxDistance;
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