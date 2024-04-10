using UnityEngine;

public class Grounded : BaseState
{
    protected MovementSM sm;

    public Grounded(string name, MovementSM stateMachine) : base(name, stateMachine)
    {
        sm = (MovementSM)this.stateMachine;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        Debug.Log("update jump Grounded");
        if (PlayerController.Instance.isGrounded)
        {
            PlayerController.Instance.isJumping = false;
        }

        if (PlayerController.Instance._input.jump && !PlayerController.Instance.isJumping)
        {
            stateMachine.ChangeState(sm.jumpingState);
        }
    }
}
