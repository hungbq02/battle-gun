using UnityEngine;

public class LandingState : Grounded
{
    float timePassed;
    float landingTime; //Delay from jumpend -> move
    public LandingState(PlayerController _playerController, MovementSM _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        timePassed = 0f;
        playerController.animator.SetTrigger("land");
        landingTime = 0.15f;
    }

    public override void UpdateLogic()
    {
        if (timePassed > landingTime)
        {
            playerController.animator.SetTrigger("move");
            //standing + jump -> standing
            //ahooting + jump -> shooting
            if (isShooting)
            {
                stateMachine.ChangeState(sm.shootState);
                return;
            }
            stateMachine.ChangeState(sm.idleState);
        }
        timePassed += Time.deltaTime;
    }
}

