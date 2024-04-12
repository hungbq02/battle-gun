using System;

public class JumpState : BaseState
{
    private MovementSM _sm;


    public JumpState(MovementSM stateMachine) : base("Jump", stateMachine)
    {
        _sm = (MovementSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        //Play Anim run
        PlayerController.Instance._animator.CrossFade(PlayerController.PLAYER_JUMP, 0.1f);
        Console.WriteLine("ENter Jump");

        PlayerController.Instance.Jump();


    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        //Check Jumping
        if (PlayerController.Instance.isGrounded && PlayerController.Instance._direction.y < 0.0f)
            PlayerController.Instance.isJumping = false;

        //JumpState -> IdleState
        if (!PlayerController.Instance.isJumping)
            stateMachine.ChangeState(_sm.idleState);
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

    }
    public override void Exit()
    {
        base.Exit();
        PlayerController.Instance._input.jump = false;

    }
}
