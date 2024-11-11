using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementSM : StateMachine
{
    [HideInInspector] public IdleState idleState;
    [HideInInspector] public MoveState moveState;
    [HideInInspector] public JumpState jumpingState;
    [HideInInspector] public ShootState shootState;
    [HideInInspector] public LandingState landingState;
    [HideInInspector] public RollingState rollingState;

    public PlayerController playerController;
    private void Awake()
    {
        idleState = new IdleState(playerController ,this);
        moveState = new MoveState(playerController, this);
        jumpingState = new JumpState(playerController, this);
        shootState = new ShootState(playerController, this);
        landingState = new LandingState(playerController, this);
        rollingState = new RollingState(playerController, this);
    }
    protected override BaseState GetInitialState()
    {
        return idleState;
    }
}
