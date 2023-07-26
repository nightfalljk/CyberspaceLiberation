
using Unity.VisualScripting;

public class XNodeLasers : XNodeLeaf
{

    public float MinDistance = 1;
    private float last_minDistance=0;
    
    private void Reset()
	{
		name = "Lasers";
	}

	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        btNode = new BTLasers(bt);
        last_minDistance = MinDistance;
        OnWriteValuesToBTNode();
        return btNode;
    }

    public override void OnWriteValuesToBTNode()
    {
        BTLasers bT_Lasers = btNode as BTLasers;
        bT_Lasers._MinDistance = MinDistance;
    }

    protected override bool CheckForNodeChanges()
    {
        bool change = false;
        return change;
    }
    protected override bool OnCheckForConfigChanges()
    {
        bool change = false;
        //Check for value change of properties
        if (last_minDistance != MinDistance)
        {
            change = true;
            last_minDistance = MinDistance;
        }
        return change;
    }
    
    public override void OnLoadEntity(Entity config)
    {
        base.OnLoadEntity(config);
        if (config == null)
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