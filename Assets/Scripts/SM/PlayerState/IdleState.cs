using UnityEngine;

public class IdleState : Grounded
{
    public IdleState(MovementSM stateMachine) : base("Idle", stateMachine) { }

    public override void Enter()
    {
        base.Enter();
  
        PlayerController.Instance._animator.Play("IdleBattle01_AR_Anim");

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
