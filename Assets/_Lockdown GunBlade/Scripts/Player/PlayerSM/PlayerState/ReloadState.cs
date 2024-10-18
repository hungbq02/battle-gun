using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ReloadState : Grounded
{
    public ReloadState(PlayerController _playerController, MovementSM _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }
    public override void Enter()
    {
        playerController.moveSpeed = 3f;
        playerController.jumpHeight = 3f;
        base.Enter();
    }
    public override void HandleInput()
    {
        base.HandleInput();
        HandleMovement();

    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if(!playerController.weapon.isReloading)
        {
       //     playerController.SetAnimLayer("UpperBodyLayer", 0f);
            stateMachine.ChangeState(sm.idleState);

        }

    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
