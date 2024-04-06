using UnityEngine;

public class MoveState : Grounded
{

    public MoveState(MovementSM stateMachine) : base("MoveState", stateMachine) { }
    public override void Enter()
    {
        base.Enter();
        Vector3 inputDirection = Vector3.zero;
       
            PlayerController.Instance._animator.Play("RunFWD_AR_Anim");
        
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Vector3 inputDirection = new Vector3(PlayerController.Instance._input.move.x, 0.0f, PlayerController.Instance._input.move.y).normalized;
        if (inputDirection.sqrMagnitude == 0f)
        {
            stateMachine.ChangeState(sm.idleState);
        }
        PlayerController.Instance.Run();

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
}
