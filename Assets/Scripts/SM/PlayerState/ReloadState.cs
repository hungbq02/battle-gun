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
        playerController.MoveSpeed = 3f;
        playerController.jumpHeight = 3f;
        base.Enter();
        moveXParameter = playerController.MoveXAnimationParameterID;
        moveZParameter = playerController.MoveZAnimationParameterID;

        playerController.animator.CrossFade("Reloading", 0.1f, 1, 0f);
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
    }
    public override void Exit()
    {
        base.Exit();
    }
}
