using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class ReloadState : BaseState
{
    int moveXParameter;
    int moveZParameter;
    public ReloadState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }
    public override void Enter()
    {
        playerController.moveSpeed = 3f;
        playerController.jumpHeight = 3f;
        base.Enter();
        moveXParameter = playerController.moveXAnimationParameterID;
        moveZParameter = playerController.moveZAnimationParameterID;
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
        if(!playerController.weapon.isReloading)
        {
       //     playerController.SetAnimLayer("UpperBodyLayer", 0f);
            stateMachine.ChangeState(playerController.standingState);

        }

    }
    public override void Exit()
    {
        base.Exit();
    }
}
