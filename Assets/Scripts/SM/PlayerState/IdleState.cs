using UnityEngine;

public class IdleState : State
{
    public IdleState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        Debug.Log("IDLE STATE");

        if (Input.GetKeyDown(KeyCode.Space))
            _player.ChangeState(new JumpState(_player));
    }
}
