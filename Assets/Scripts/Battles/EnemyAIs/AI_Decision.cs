using System.Collections.Generic;

public class AI_Decision : AI_Base
{
    public enum DecisionType { And, Or }
    List<AI_Base> childrenNodes = new List<AI_Base>();

    DecisionType decisionType;

    public void Initialize(AI_Type _ai_Type, DecisionType _decisionType)
    {
        Initialize(_ai_Type);
        decisionType = _decisionType;
    }

    protected override AI_State Work()
    {
        if (ai_State != AI_State.Success)
        {
            ResetState();
            return AI_State.Fail;
        }

        for (int i = 0; i < childrenNodes.Count; i++)
        {
            AI_State childState = childrenNodes[i].Excute();

            if(decisionType == DecisionType.Or)
            {
                if (childState == AI_State.Success)
                {
                    return AI_State.Success;
                }
            }
            else
            {
                if (childState != AI_State.Success)
                {
                    for (int j = i; i >= 0; j--)
                    {
                        childrenNodes[j].ResetState();
                    }
                    return AI_State.Fail;
                }
            }
        }
        return AI_State.Success;
    }

    public void AddChildren(AI_Base adding)
    {
        childrenNodes.Add(adding);
    }

    public void RemoveChildren(AI_Base removeing)
    {
        childrenNodes?.Remove(removeing);
    }
    public override void ResetState()
    {
        ai_State = AI_State.Ready;
    }
}