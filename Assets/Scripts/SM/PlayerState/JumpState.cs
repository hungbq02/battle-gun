using UnityEngine;

public class JumpState : State
{
    public JumpState(PlayerController player) : base(player) { }
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;
    private float _verticalVelocity;

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
        Debug.Log("JUMP STATE");
        if(_player.Grounded)
        {
            _player.ChangeState(new IdleState(_player));
        }    
    }
}
