using UnityEngine;

public abstract class BaseState
{
    public PlayerController playerController;

    protected Vector3 velocity;
    protected Vector3 gravityVelocity;

    protected float gravity;

    protected StateMachine stateMachine;

    bool isGrounded;
    public BaseState(PlayerController playerController, StateMachine stateMachine)
    {
        this.playerController = playerController;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter() { }
    public virtual void HandleInput() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}