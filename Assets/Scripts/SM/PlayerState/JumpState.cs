using System;
using UnityEngine;

public class JumpState : BaseState
{
    private MovementSM _sm;
    private bool _grounded;

    // private int _groundLayer = 1 << 6;

    public JumpState(MovementSM stateMachine) : base("Jump", stateMachine)
    {
        _sm = (MovementSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        PlayerController.Instance._animator.Play("Jump_AR_Anim");
        Console.WriteLine("ENter Jump");
        PlayerController.Instance.Jump();
        

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (!PlayerController.Instance.isJumping)
        {
            Console.WriteLine("Change state jump to idle in jumpsate");

            stateMachine.ChangeState(_sm.idleState);
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
      //  _grounded = PlayerController.Instance.isGrounded;
    }
}
