
public class XNodeCheckValue : XNodeLeaf
{

    public string VariableName;
    public BTCheckValue.Operator Operator;
    public float Value;
    
    private void Reset()
	{
		name = "CheckValue";
	}

	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        btNode = new BTCheckValue(bt);
        OnWriteValuesToBTNode();
        return btNode;
    }

    public override void OnWriteValuesToBTNode()
    {
        BTCheckValue bTCheckValue = btNode as BTCheckValue;
        bTCheckValue.VaraibleName = VariableName;
        bTCheckValue.Op = Operator;
        bTCheckValue.Value = Value;
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