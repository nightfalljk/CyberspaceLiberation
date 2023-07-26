using System;
using XNode;

public class BTCheckValue : BTNode
{

    public string VaraibleName;
    public Operator Op;
    public float Value;
    
	public BTCheckValue(BehaviourTree bt) : base(bt)
    {
    }
    
    public override Result Execute(int instanceID, bool init = false)
    {
        //return base.Execute(instanceID, init);
        float valueFromName;
        if (Tree.GetBBValue<float>(instanceID, VaraibleName, out valueFromName))
        {
            switch (Op)
            {
                case Operator.Eq:
                    NodeState = valueFromName == Value ? Result.Success : Result.Failure;
                    break;
                case Operator.UEq:
                    NodeState = valueFromName != Value ? Result.Success : Result.Failure;
                    break;
                case Operator.Gt:
                    NodeState = valueFromName > Value ? Result.Success : Result.Failure;
                    break;
                case Operator.GEt:
                    NodeState = valueFromName >= Value ? Result.Success : Result.Failure;
                    break;
                case Operator.St:
                    NodeState = valueFromName < Value ? Result.Success : Result.Failure;
                    break;
                case Operator.SEt:
                    NodeState = valueFromName <= Value ? Result.Success : Result.Failure;
                    break;
                default:
                    NodeState = Result.Failure;
                    throw new ArgumentOutOfRangeException();
            }
            //NodeState = Result.Success;
        }
        else
        {
            NodeState = Result.Failure;
        }
        
        Tree.SetStateValue(instanceID, this, NodeState);
        return NodeState;
    }
    
    public enum Operator
    {
        Eq,
        UEq,
        Gt,
        GEt,
        St,
        SEt
    }
}