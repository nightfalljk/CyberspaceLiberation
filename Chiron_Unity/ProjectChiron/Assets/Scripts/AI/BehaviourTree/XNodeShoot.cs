
public class XNodeShoot : XNodeLeaf {

	protected override BTNode CreateBtNodeAndNext(BehaviourTree bt)
    {
        btNode = new BTShoot(bt);
        OnWriteValuesToBTNode();
        return btNode;
    }

    private void Reset()
    {
        name = "Shoot";
    }

    public override void OnWriteValuesToBTNode()
    {
        BTShoot bT_Change = btNode as BTShoot;
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
        //Set values from Config
    }
    
    protected override void OnWriteConfigValues()
    {
    }
}