using UnityEngine;
using UnityEngine.InputSystem;

public class RollingState : BaseState
{
    int moveXParameter;
    int moveZParameter;


    public float rollSpeed = 10f;
    public float rollDuration = 0.7f;
    private float rollTime = 0.0f;
    private Vector3 rollDirection;

    public RollingState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        moveXParameter = playerController.moveXAnimationParameterID;
        moveZParameter = playerController.moveZAnimationParameterID;
        rollTime = 0.0f;


    }
    public override void HandleInput()
    {
        base.HandleInput();

    }
    public override void UpdateLogic()
    {
        Debug.Log("roll time :" + rollTime + " /roll duration " + rollDuration);
        gravityVelocity.y += gravity * Time.deltaTime;

        if (velocity.magnitude == 0)
        {
            playerController.input.roll = false;
            stateMachine.ChangeState(playerController.standingState);
            return;
        }
        if (rollTime > rollDuration)
        {
            playerController.input.roll = false;
            stateMachine.ChangeState(playerController.standingState);
            Debug.Log("FALSE ROLLING");
        }
        else
        {
            playerController.animator.SetBool("isRolling", true);
            playerController.animator.SetFloat(moveXParameter, playerController.input.move.x);
            playerController.animator.SetFloat(moveZParameter, playerController.input.move.y);

            //move player
            rollDirection = (velocity * rollSpeed) + (Vector3.up * gravityVelocity.y);
            playerController.controller.Move(rollDirection * Time.deltaTime);

            //Change collider player
            playerController.controller.height = 1.3f;
            playerController.controller.center = new Vector3(0f, 0.5f, 0f);

            rollTime += Time.deltaTime;

        }
    }
    public override void Exit()
    {
        base.Exit();
        playerController.animator.SetBool("isRolling", false);
        //Reset collider
        playerController.controller.height = 1.8f;
        playerController.controller.center = new Vector3(0f, 0.93f, 0f);


    }
}
