using UnityEngine;

public class MovementSM : StateMachine
{
    public float speed = 4f;
    public float jumpForce = 14f;

    [HideInInspector]
    public IdleState idleState;
    [HideInInspector]
    public MoveState moveState;
    [HideInInspector]
    public JumpState jumpingState;

    private void Awake()
    {
        idleState = new IdleState(this);
        moveState = new MoveState(this);
        jumpingState = new JumpState(this);
    }

    protected override BaseState GetInitialState()
    {
        return idleState;
    }
}
