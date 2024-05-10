using UnityEngine;

public class StandingState : BaseState
{
    bool jump;
    bool shoot;

    int moveXParameter;
    int moveZParameter;

    private float smoothMoveX;
    private float smoothMoveZ;
    float velocityX;
    float velocityZ;

    private float smoothTime = 0.2f;
    public StandingState(PlayerController _playerController, StateMachine _stateMachine) : base( _playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        jump = false;
        shoot = false;

        moveXParameter = playerController.MoveXAnimationParameterID;
        moveZParameter = playerController.MoveZAnimationParameterID;

    }

    public override void HandleInput()
    {
        base.HandleInput();

        if(playerController.input.jump)
        {
            jump = true;
        }
        if (playerController.input.shoot)
        {
            shoot = true;
        }

        //Anim
        smoothMoveX = Mathf.SmoothDamp(smoothMoveX, playerController.input.move.x, ref velocityX, smoothTime);
        smoothMoveZ = Mathf.SmoothDamp(smoothMoveZ, playerController.input.move.y, ref velocityZ, smoothTime);
        playerController.animator.SetFloat(moveXParameter, smoothMoveX);
        playerController.animator.SetFloat(moveZParameter, smoothMoveZ);

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (jump)
        {
            stateMachine.ChangeState(playerController.jumpingState);
        }
        if (shoot)
        {
            stateMachine.ChangeState(playerController.shootState);
        }
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
    public override void Exit()
    {
        base.Exit();

      //  gravityVelocity.y = 0f;

    }
}
