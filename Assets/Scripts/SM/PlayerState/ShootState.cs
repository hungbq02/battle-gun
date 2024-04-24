public class ShootState : BaseState
{
    private StateMachine sm;

    public ShootState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
      //  PlayerController.Instance.animator.CrossFade(PlayerController.PLAYER_SHOT, 0.1f);
     //   PlayerController.Instance.Shoot();

    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

     //   if (PlayerController.Instance.input.shoot)
       //     PlayerController.Instance.Shoot();

        /*        if(!PlayerController.Instance.input.shoot)
                {
                    stateMachine.ChangeState(sm.idleState);
                }*/
    }
    public override void Exit()
    {
        base.Exit();
    }
}
