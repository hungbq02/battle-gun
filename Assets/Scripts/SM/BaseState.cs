using UnityEngine;

public class BaseState
{
    public PlayerController playerController;

    protected StateMachine stateMachine;
    protected Vector3 velocity;
    protected Vector3 gravityVelocity;


    public BaseState(PlayerController playerController, StateMachine stateMachine)
    {
        this.playerController = playerController;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter(){ }
    public virtual void HandleInput() { }
    public virtual void UpdateLogic() { Debug.Log("Current State: " + this.ToString()); }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}
