
public class XNodeRotate : XNodeLeaf
{

    public float angle;
    public float speed;

    private float lastAngle;
    private float lastSpeed;
    private void Reset()
	{
		name = "Rotate";
	}

	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        btNode = new BTRotate(bt);
        OnWriteValuesToBTNode();
        return btNode;
    }

    public override void OnWriteValuesToBTNode()
    {
        BTRotate bTRotate = btNode as BTRotate;
        bTRotate.angle = angle;
        bTRotate.speed = speed;
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
        if (lastAngle != angle)
        {
            lastAngle = angle;
            change = true;
        }
        if (lastSpeed != speed)
        {
            lastSpeed = speed;
            change = true;
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