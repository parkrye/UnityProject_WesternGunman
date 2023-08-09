public class AI_Action : AI_Base
{
    protected override AI_State Work()
    {
        return ai_State;
    }

    public override void ResetState()
    {
        ai_State = AI_State.Ready;
    }
}
