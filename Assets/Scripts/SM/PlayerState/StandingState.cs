using UnityEngine;

public class StandingState : BaseState
{
    float gravity;

    bool jump;
    bool shoot;

    int moveXParameter;
    int moveZParameter;

    bool isGrounded;
    float playerSpeed;
    Vector3 moveDir = Vector3.zero;
    public float verticalVelocity;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    public StandingState(PlayerController _playerController, StateMachine _stateMachine) : base( _playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();

        jump = false;
        shoot = false;
       // playerController.input.move = Vector2.zero;
        playerSpeed = playerController.MoveSpeed;
        isGrounded = playerController.isGrounded();
        gravity = playerController.gravity;

        moveXParameter = playerController.MoveXAnimationParameterID;
        moveZParameter = playerController.MoveZAnimationParameterID;

    }



    public override void HandleInput()
    {
        base.HandleInput();
        if(playerController.input.jump)
        {
            jump = true;
        }         
        if(playerController.input.shoot)
        {
            shoot = true;
        }

        playerController.animator.SetFloat(moveXParameter, playerController.input.move.x);
        playerController.animator.SetFloat(moveZParameter, playerController.input.move.y);


        velocity = new Vector3(playerController.input.move.x, 0.0f, playerController.input.move.y).normalized;
        if (velocity.sqrMagnitude == 0f) return;
        float targetAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg + playerController.cameraTransform.eulerAngles.y;

        moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        velocity = moveDir;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if(jump)
        {
            stateMachine.ChangeState(playerController.jumpingState);
        }
        if (shoot)
        {
            stateMachine.ChangeState(playerController.shootState);
        }

        gravityVelocity.y += gravity * Time.deltaTime;
        isGrounded = playerController.isGrounded();

        if (isGrounded && gravityVelocity.y < 0.0f)
        {
            gravityVelocity.y = -1.0f;
        }
        //add
        playerController.controller.Move(velocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);
        playerController.RotateTowardsCamera();
        Debug.Log("update logic standing State: ");

    }

    public override void UpdatePhysics()
    {
        base.UpdatePhysics();


    }
    public override void Exit()
    {
        base.Exit();

        gravityVelocity.y = 0f;

    }
/*    private void RotateTowardsCamera()
    {
        // Get the angle between the character's current forward direction and the camera's forward direction
        float targetAngle = Mathf.Atan2(playerController.cameraTransform.forward.x, playerController.cameraTransform.forward.z) * Mathf.Rad2Deg;

        // Smoothly rotate the character towards the camera's forward direction
        float angle = Mathf.SmoothDampAngle(playerController.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        playerController.transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }*/
}
