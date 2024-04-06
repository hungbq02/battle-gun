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
        if (!Input.GetKeyDown(KeyCode.V) || !PlayerController.Instance.isGrounded) return;
        if (!PlayerController.Instance.isJumping)
        {
            stateMachine.ChangeState(sm.jumpingState);
            PlayerController.Instance.Jump();
        }
    }
}
