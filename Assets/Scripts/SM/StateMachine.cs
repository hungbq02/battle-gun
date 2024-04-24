
using UnityEngine;

public class StateMachine
{
    public BaseState currentState;
    public void Initialize(BaseState startingState)
    {
        currentState = startingState;
        startingState.Enter();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();

        currentState = newState;
        newState.Enter();
    }

}
