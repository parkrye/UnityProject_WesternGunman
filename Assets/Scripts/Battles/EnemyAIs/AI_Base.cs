using UnityEngine;

public abstract class AI_Base : ScriptableObject
{
    public enum AI_Type { Action, Decision }
    public enum AI_State { Ready, Running, Success, Fail }

    protected AI_Type ai_Type;
    protected AI_State ai_State;

    public void Initialize(AI_Type _ai_Type)
    {
        ai_Type = _ai_Type;
    }

    public AI_State Excute()
    {
        ai_State = Work();
        if (ai_State != AI_State.Success)
        {
            ResetState();
            return AI_State.Fail;
        }

        return AI_State.Success;
    }

    protected abstract AI_State Work();

    public abstract void ResetState();
}
