using UnityEngine;

public class ShootState : BaseState
{
    int moveXParameter;
    int moveZParameter;

    private float smoothMoveX;
    private float smoothMoveZ;
    float velocityX;
    float velocityZ;

    private float smoothTime = 0.2f;
    public ShootState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        playerController.MoveSpeed = 3f;
        playerController.jumpHeight = 3f;
        base.Enter();
        moveXParameter = playerController.MoveXAnimationParameterID;
        moveZParameter = playerController.MoveZAnimationParameterID;
    }
    public override void HandleInput()
    {
        base.HandleInput();

        smoothMoveX = Mathf.SmoothDamp(smoothMoveX, playerController.input.move.x, ref velocityX, smoothTime);
        smoothMoveZ = Mathf.SmoothDamp(smoothMoveZ, playerController.input.move.y, ref velocityZ, smoothTime);
        playerController.animator.SetFloat(moveXParameter, smoothMoveX);
        playerController.animator.SetFloat(moveZParameter, smoothMoveZ);
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (playerController.input.shoot)
        {
            //  playerController.input.aim = true;
            playerController.SetAnimLayer("aiming", 1f);
            if (playerController.weapon.canShoot)
            {
                playerController.animator.Play("ShootSingleshot", 1, 0f);
                playerController.weapon.StartShooting();
            }
            playerController.input.shoot = false;
           // playerController.movementSM.ChangeState(playerController.standingState);
        }
        if (playerController.input.jump)
        {
            stateMachine.ChangeState(playerController.jumpingState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

}
