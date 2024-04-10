using System;

public class JumpState : BaseState
{
    private MovementSM _sm;

    // private int _groundLayer = 1 << 6;

    public JumpState(MovementSM stateMachine) : base("Jump", stateMachine)
    {
        _sm = (MovementSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        PlayerController.Instance._animator.CrossFade(PlayerController.PLAYER_JUMP, 0.1f);
        Console.WriteLine("ENter Jump");
        PlayerController.Instance.Jump();


    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (PlayerController.Instance.isGrounded && PlayerController.Instance._direction.y < 0.0f)
            PlayerController.Instance.isJumping = false;

        if (!PlayerController.Instance.isJumping)
        {
            Console.WriteLine("Change state jump to idle in jumpsate");
            stateMachine.ChangeState(_sm.idleState);
        }

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();

    }
}
