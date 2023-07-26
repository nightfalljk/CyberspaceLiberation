using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using XNode;

public class XNodeBT : Node {
	
	
	//corresponding BTNode
	protected BTNode btNode;
	//protected int selectedInstanceId;
	protected Entity entity;

	protected XNodeGraph XNodeGraph;
	
	// Use this for initialization
	protected override void Init() {
		base.Init();
		XNodeGraph = graph as XNodeGraph;
		XNodeGraph.LateLoadEntity(this);
	}

	public override void OnCreateConnection(NodePort @from, NodePort to)
	{
		base.OnCreateConnection(@from, to);
		//Debug.Log($"connection from {@from.fieldName} to {to.fieldName}");
	}

	public override void OnRemoveConnection(NodePort port)
	{
		base.OnRemoveConnection(port);
	}

	// Return the correct value of an output port when requested
	public override object GetValue(NodePort port) {
		return null; // Replace this
	}

	public BTNode GetBtNode(BehaviourTree bt)
	{
		if (!Application.isPlaying)
		{
			Debug.LogWarning("Requesting BT Node while in Editor");
			return null;
		}
		if (btNode == null)
			btNode = CreateBtNodeAndNext(bt);
		
		return btNode;
	}

	protected virtual BTNode CreateBtNodeAndNext(BehaviourTree bt)
	{
		return null; //CreateBtNode(bt, null);
	}

//	public virtual BTNode CreateBtNode(BehaviourTree bt, BTNode child)
//	{
//		Debug.Log("No CreateBtNode on"+ (this.GetType().Name));
//		return null;
//	}
	
	public List<XNodeBT> GetNextXNodes()
	{
		List<XNodeBT> nextNodes = new List<XNodeBT>();
		
		//Take all connections of all output ports; maybe change to allow nonBT output ports; or ignore because Type is checked
		foreach (NodePort nodePort in Outputs)
		{
			foreach (NodePort connectedPort in nodePort.GetConnections())
			{
				XNodeBT xNodeBt = connectedPort.node as XNodeBT;
				nextNodes.Add(xNodeBt);
			}
		}
		nextNodes.Sort((x, y)=> x.position.y.CompareTo(y.position.y));
		return nextNodes;
	}

	public List<BTNode> GetNextBtNodes(BehaviourTree bt)
	{
		List<XNodeBT> nextXNodes = GetNextXNodes();
		List<BTNode> nextBtNodes = new List<BTNode>();
		foreach (XNodeBT nextXNode in nextXNodes)
		{
			nextBtNodes.Add(nextXNode.GetBtNode(bt));
		}
		return nextBtNodes;
	}

	protected virtual void OnValidate()
	{
		if (OnCheckForConfigChanges())
		{
			if(entity==null)
				return;
			
			if (btNode != null && btNode.Tree.UpdateLiveValues)
			{
				OnWriteValuesToBTNode();
			}
			OnWriteConfigValues();
			if (btNode != null)
			{
				btNode.Tree.LoadParametersToBlackboard(entity);
			}
		}

		if (CheckForNodeChanges())
		{
			//reload entity
			OnLoadEntity(entity);
		}
	}

//	public void ChangeViewingInstanceID(int id)
//	{
//		selectedInstanceId = id;
//	}

	//TODO check use of this function
	public virtual void OnWriteValuesToBTNode()
	{
	} 
	
	public virtual void OnLoadEntity(Entity entity)
	{
		this.entity = entity;
	}

	protected virtual void OnWriteConfigValues()
	{
	}
	
	protected virtual bool CheckForNodeChanges()
	{
		return false;
	}

	protected virtual bool OnCheckForConfigChanges()
	{
		return false;
	}

}