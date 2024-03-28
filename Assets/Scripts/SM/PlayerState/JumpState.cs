using UnityEngine;

public class JumpState : State
{
    public JumpState(PlayerController player) : base(player) { }

    public override void Enter()
    {
        Debug.Log("ENTER JUMP" + "JumH = "+_player.JumpHeight +"------Gravity = "+ _player.Gravity);

        _player._verticalVelocity = Mathf.Sqrt(_player.JumpHeight * -2f * _player.Gravity);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        if (_player.Grounded)
        {
            Debug.Log("IDLE STATE");
            _player.ChangeState(new IdleState(_player));
        }
    }
}
