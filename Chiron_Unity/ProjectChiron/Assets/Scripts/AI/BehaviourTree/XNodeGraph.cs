using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "BT_Graph")]
public class XNodeGraph : NodeGraph
{
	public BehaviourTree bt = null;
	private XNodeBtRoot root = null;
	[SerializeField]
	private Entity selectedEntity = null;
	
	//Serialize to carry from editor to runtime for NodeEditor reference
	[SerializeField]
	private int selectedEntityId = 0;

	//Not called?! After CodeReload?
	private void Awake()
	{
		GetRoot()?.SetIdWithName(GetRoot().EntityName);
	}

	
#if UNITY_EDITOR
	//When exactly? Not at beginning of runtime?!
	private void OnEnable()
	{
		GetRoot()?.SetIdWithName(GetRoot().EntityName);
		EditorApplication.playModeStateChanged += ModeChanged;
	}
	//Load Entity back in Editor
	void ModeChanged(PlayModeStateChange playModeState)
	{
		if (playModeState == PlayModeStateChange.EnteredEditMode) 
		{
			//Debug.Log("Entered Edit mode.");
			GetRoot().SetIdWithName(GetRoot().EntityName);
		}

		if (playModeState == PlayModeStateChange.EnteredPlayMode)
		{
			GetRoot().SetIdWithName(GetRoot().EntityName);
		}
	}
#endif
	
	private void OnDisable()
	{
		//GetRoot().SetIdWithName(GetRoot().EntityName);
	}

	//Create Graph
	public void Reset()
	{
		
	}

	public XNodeBtRoot GetRoot()
	{
		if (root == null)
		{
			foreach (XNodeBT node in nodes)
			{
				root = node as XNodeBtRoot;
				break;//TODO Check for multiple roots
			}
		}

		return root;
	}

	public void LoadEntity(Entity entity)
	{
		selectedEntity = entity;
		if (entity == null)
		{
			selectedEntityId = 0;
		}
		else
		{
			selectedEntityId = entity.GetInstanceID();
		}
		foreach (XNodeBT node in nodes)
        {
        	node.OnLoadEntity(entity);
        }
	}

	public void LateLoadEntity(XNodeBT node)
	{
		node.OnLoadEntity(selectedEntity);
	}

	public int GetSelectedInstanceId()
	{
//		if (selectedEntity == null)
//			return 0;
//		return selectedEntity.GetInstanceID();
		return selectedEntityId;
	}
}