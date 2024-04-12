using UnityEngine;

public class IdleState : Grounded
{
    public IdleState(MovementSM stateMachine) : base("Idle", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        PlayerController.Instance._direction = Vector3.zero;

        PlayerController.Instance._animator.CrossFade(PlayerController.PLAYER_IDLE, 0.1f);
        Debug.Log("ENter Idle");

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Vector3 inputDirection = new Vector3(PlayerController.Instance._input.move.x, 0.0f, PlayerController.Instance._input.move.y).normalized;
        if (inputDirection.sqrMagnitude >= 0.1f)
        {
            stateMachine.ChangeState(sm.moveState);
        }

    }
}
