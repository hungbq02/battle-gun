using UnityEngine;

public class LandingState : BaseState
{
    float timePassed;
    float landingTime;
    public LandingState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        timePassed = 0f;
        landingTime = 0f;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        if (timePassed > landingTime)
        {
            stateMachine.ChangeState(playerController.standingState);
        }
        timePassed += Time.deltaTime;
    }
}

