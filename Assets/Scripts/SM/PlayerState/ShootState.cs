using UnityEngine;

public class ShootState : BaseState
{
    public ShootState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        playerController.MoveSpeed = 3f;
        base.Enter();
    }
    public override void HandleInput()
    {
        base.HandleInput();
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (playerController.input.shoot)
        {
            playerController.animator.SetTrigger("shoot");
            playerController.weapon.StartShooting();
            playerController.input.shoot = false;
            playerController.movementSM.ChangeState(playerController.standingState);
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

}