
public class XNodeAnimationTrigger : XNodeLeaf {

    public string TriggerName;
    private string lastTriggerName;
    
    private void Reset()
	{
		name = "TriggerAnimation";
	}

	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        btNode = new BTAnimationTrigger(bt);
        OnWriteValuesToBTNode();
        return btNode;
    }

    public override void OnWriteValuesToBTNode()
    {
        BTAnimationTrigger bT_AnimationTrigger = btNode as BTAnimationTrigger;
        bT_AnimationTrigger.TriggerName = TriggerName;
    }

    protected override bool CheckForNodeChanges()
    {
        bool change = false;
        //Check for value change of properties
        return change;
    }
    
    protected override bool OnCheckForConfigChanges()
    {
        bool change = false;
        //Check for value change of properties
        if (lastTriggerName != TriggerName)
        {
            change = true;
            lastTriggerName = TriggerName;
        }
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
        //Value = entity.GetParameter<float>(ValueName);
    }
    
    protected override void OnWriteConfigValues()
    {
        //entity.SetParameter(ValueName, Value);
    }
}