using UnityEngine;
using UnityEngine.InputSystem;

public class RollingState : Grounded
{
    public float rollSpeed = 10f;
    public float rollDuration = 0.7f;
    private float rollTime = 0.0f;
    private Vector3 rollDirection;

    public RollingState(PlayerController _playerController, MovementSM _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }
    public override void Enter()
    {
        base.Enter();

        rollTime = 0.0f;
        isRolling = true;

    }
    public override void HandleInput()
    {
        base.HandleInput();
        HandleMovement();
    }
    public override void UpdateLogic()
    {
        /*Debug.Log("roll time :" + rollTime + " /roll duration " + rollDuration);*/
        gravityVelocity.y += gravity * Time.deltaTime;

        if (normInput.magnitude == 0)
        {
            //reset animation
            playerController.animator.SetBool("isRolling", false);

            stateMachine.ChangeState(sm.idleState);
            return;
        }
        if (rollTime > rollDuration)
        {
            stateMachine.ChangeState(sm.idleState);
           // Debug.Log("FALSE ROLLING");
        }
        else
        {
            playerController.animator.SetBool("isRolling", true);
            UpdateMovementAnimation();

            //move player
            rollDirection = (moveDir * rollSpeed) /*+ (Vector3.up * gravityVelocity.y)*/;
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
        isRolling = false;
        //Reset collider
        playerController.controller.height = 1.8f;
        playerController.controller.center = new Vector3(0f, 0.93f, 0f);


    }
}
