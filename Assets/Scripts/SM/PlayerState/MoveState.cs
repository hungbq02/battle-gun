using UnityEngine;

public class MoveState : Grounded
{

    public MoveState(MovementSM stateMachine) : base("MoveState", stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Vector2 input = PlayerController.Instance._input.move;

        PlayerController.Instance._animator.CrossFade(PlayerController.PLAYER_RUN_FWD, 0.1f);

        /*        if (input.y == 1f)
                {
                    PlayerController.Instance._animator.CrossFade(PlayerController.PLAYER_RUN_FWD, 0.1f);
                }
                else if (input.y == -1f)
                {
                    PlayerController.Instance._animator.CrossFade(PlayerController.PLAYER_RUN_BWD, 0.1f);
                }
                else if (input.x == 1f)
                {
                    PlayerController.Instance._animator.CrossFade(PlayerController.PLAYER_RUN_RIGHT, 0.1f);
                }
                else if (input.x == -1f)
                {
                    PlayerController.Instance._animator.CrossFade(PlayerController.PLAYER_RUN_LEFT, 0.1f);
                }*/

    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Vector3 inputDirection = new Vector3(PlayerController.Instance._input.move.x, 0.0f, PlayerController.Instance._input.move.y).normalized;
        if (inputDirection.sqrMagnitude == 0f)
        {
            stateMachine.ChangeState(sm.idleState);
        }
         PlayerController.Instance.Move();

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
}
