using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public class XNodeSequencer : XNodeComposite
{
	public BTNode.ProcessType ProcessType = BTNode.ProcessType.SaveSequenceState;
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
		
	}

	private void Reset()
	{
		name = "Sequencer";
	}
	
	public override BTNode CreateBtNodes(BehaviourTree bt, List<BTNode> children)
	{
		btNode = new BTSequencer(bt, children.ToArray());
		OnWriteValuesToBTNode();
		return btNode;
	}

	protected override bool OnCheckForConfigChanges()
	{
		bool change = false;
		return change;
	}

	public override void OnWriteValuesToBTNode()
	{
		BTSequencer btSequence = btNode as BTSequencer;
		btSequence._ProcessType = ProcessType;
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