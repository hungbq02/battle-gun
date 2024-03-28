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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("JUMP STATE");
            _player.ChangeState(new JumpState(_player));

        }

    }
}
