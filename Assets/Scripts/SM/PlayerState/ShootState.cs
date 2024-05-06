using System.Collections;
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
        base.Enter();
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (playerController.input.shoot)
        {
            playerController.animator.SetTrigger("shoot");
            //     Shoot();
            playerController.weapon.StartShooting();
            playerController.input.shoot = false;
        }
    }
    public override void Exit()
    {
        base.Exit();
    }

}
