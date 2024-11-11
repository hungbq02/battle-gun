using UnityEngine;

public class IdleState : Grounded
{
    public IdleState(PlayerController _playerController, MovementSM _stateMachine) : base(_playerController, _stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        playerController.moveSpeed = 6f;
        playerController.jumpHeight = 5f;
    }

    public override void HandleInput()
    {
        base.HandleInput();
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        UpdateMovementAnimation();
         
        if (inputDir.sqrMagnitude > Mathf.Epsilon)
        {
            stateMachine.ChangeState(sm.moveState);
            return;
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
