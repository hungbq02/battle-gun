using UnityEngine;

public class MoveState : Grounded
{
    float animationSpeed;

    public MoveState(PlayerController _playerController, MovementSM _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        moveSpeed = playerController.moveSpeed;
    }

    public override void HandleInput()
    {
        base.HandleInput();

        moveSpeed = playerController.input.sprint ? playerController.sprintSpeed : playerController.moveSpeed;
        animationSpeed = playerController.input.sprint ? 2.0f : 1.0f; // x2 speed animation move
        playerController.animator.SetFloat("MoveAnimSpeed", animationSpeed);
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        HandleMovement();

        //Roll
        if (playerController.input.roll)
        {
            stateMachine.ChangeState(sm.rollingState);
            return;
        }
/*        //Idle
        if (inputDir.sqrMagnitude == 0f)
        {
            stateMachine.ChangeState(sm.idleState);
            return;
        }*/
    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
}
