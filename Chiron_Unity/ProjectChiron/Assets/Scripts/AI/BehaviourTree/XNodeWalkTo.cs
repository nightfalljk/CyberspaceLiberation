
public class XNodeWalkTo : XNodeLeaf {

    private void Reset()
	{
		name = "WalkTo";
	}

	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        btNode = new BTWalkTo(bt);
        OnWriteValuesToBTNode();
        return btNode;
    }

    public override void OnWriteValuesToBTNode()
    {
        BTWalkTo bT_walkTo = btNode as BTWalkTo;
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