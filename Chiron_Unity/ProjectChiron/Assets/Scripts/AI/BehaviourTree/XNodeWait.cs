
using UnityEngine;
using UnityEngine.Serialization;

public class XNodeWait : XNodeLeaf
{

    public string WaitName;
    private string lastWaitName;

    [Space(10)]
    [FormerlySerializedAs("rt_waitValue")] public float WaitValue;
    private float lastWaitValue;
    
    private void Reset()
	{
		name = "Wait";
	}

	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        btNode = new BTWait(bt);
        OnWriteValuesToBTNode();
        return btNode;
    }

    public override void OnWriteValuesToBTNode()
    {
        BTWait bTWait = btNode as BTWait;
        bTWait.WaitVariableName = WaitName;
        bTWait.WaitValue = WaitValue;
    }


    protected override bool CheckForNodeChanges()
    {
        bool change = false;
        if (lastWaitName != WaitName)
        {
            change = true;
            lastWaitName = WaitName;
        }

        return change;
    }
    protected override bool OnCheckForConfigChanges()
    {
        bool change = false;
        //Check for value change of properties
        
        if (lastWaitValue != WaitValue)
        {
            change = true;
            lastWaitValue = WaitValue;
        }
        return change;
    }
    
    public override void OnLoadEntity(Entity entity)
    {
        base.OnLoadEntity(entity);
        if (entity == null)
        {
            //SetDefault
            if(!string.IsNullOrEmpty(WaitName))
                WaitValue = -1;
            return;
        }
        //Set values from Config
        
        //Change to access outside of runtime over entity -> config
        if(!string.IsNullOrEmpty(WaitName))
            WaitValue = entity.GetParameter<float>(WaitName);
//        BTWait bTWait = btNode as BTWait;
//        if (bTWait != null)
//        {
//            bTWait.Tree.GetBBValue<float>(entity.GetInstanceID(), WaitName, out rt_waitValue);
//        }
    }
    
    protected override void OnWriteConfigValues()
    {
        if(!string.IsNullOrEmpty(WaitName))
            entity.SetParameter(WaitName, WaitValue);
    }
}