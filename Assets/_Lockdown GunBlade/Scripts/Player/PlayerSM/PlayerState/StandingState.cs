using UnityEngine;

public class StandingState : BaseState
{
    bool jump;
    bool shoot;
    bool roll;



    int moveXParameter;
    int moveZParameter;

    public StandingState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        jump = false;
        shoot = false;
        roll = false;

        playerController.moveSpeed = 6f;
        playerController.jumpHeight = 5f;

        moveXParameter = playerController.moveXAnimationParameterID;
        moveZParameter = playerController.moveZAnimationParameterID;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        if (playerController.input.jump)
        {
            jump = true;
        }
        if (playerController.input.shoot)
        {
            shoot = true;
        }
        if (playerController.input.roll)
        {
            roll = true;
        }
        //Anim
        playerController.animator.SetFloat(moveXParameter, playerController.input.move.x);
        playerController.animator.SetFloat(moveZParameter, playerController.input.move.y);

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
/*        if (reload)
        {
            stateMachine.ChangeState(playerController.reloadState);
        }*/
        if (roll)
        {
            stateMachine.ChangeState(playerController.rollingState);
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
