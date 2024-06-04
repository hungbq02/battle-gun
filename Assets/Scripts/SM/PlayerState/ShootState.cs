using UnityEngine;

public class ShootState : BaseState
{
    int moveXParameter;
    int moveZParameter;


    private float transitionLayerSpeed = 14f;

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

        playerController.animator.SetFloat(moveXParameter, playerController.input.move.x);
        playerController.animator.SetFloat(moveZParameter, playerController.input.move.y);
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (playerController.input.shoot)
        {
            if (playerController.weapon.canShoot)
            {
                playerController.SetAnimLayer("UpperBodyLayer", 1f);
                playerController.animator.CrossFade("ShootSingleshot", 0.1f, 1, 0f);
                playerController.weapon.StartShooting();
            }
        }
        else
        {
            playerController.weapon.StopShooting();
            //Get layer shooting
            float currentLayerWeight = playerController.animator.GetLayerWeight(1);

            //Smooth change layer
            playerController.SetAnimLayer("UpperBodyLayer", Mathf.Lerp(currentLayerWeight, 0f, Time.deltaTime * transitionLayerSpeed));

            if (currentLayerWeight < 0.001f)
            {
                playerController.movementSM.ChangeState(playerController.standingState);
            }
        }

        //Change state
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
