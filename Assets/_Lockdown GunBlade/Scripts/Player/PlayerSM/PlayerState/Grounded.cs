using UnityEngine;

public class Grounded : BaseState
{
    protected MovementSM sm;
    protected Vector3 inputDir;
    protected Vector3 normInput;

    protected Vector3 moveDir;
    protected float moveSpeed;


    protected int moveXParameter;
    protected int moveZParameter;

    bool isGrounded;
    protected bool isRolling;
    protected bool isShooting;


    public Grounded(PlayerController playerController, MovementSM stateMachine) : base(playerController, stateMachine)
    {
        sm = (MovementSM)this.stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        gravity = playerController.gravity;

        //Get ID of the parameters
        moveXParameter = playerController.moveXAnimationParameterID;
        moveZParameter = playerController.moveZAnimationParameterID;
    }

    public override void HandleInput()
    {
        base.HandleInput();
        inputDir = new Vector3(playerController.input.move.x, 0.0f, playerController.input.move.y);
        normInput = inputDir.normalized;
    }

    public override void UpdateLogic()
    {
        base.UpdateLogic();
        
        isGrounded = playerController.IsGrounded();
        //Jump
        if (isShooting) return;
        if (playerController.input.jump)
            stateMachine.ChangeState(sm.jumpingState);
        if (playerController.input.shoot && !isRolling)
            stateMachine.ChangeState(sm.shootState);
    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
        ApplyGravity();
    }
    protected void HandleMovement()
    {
        if (inputDir.sqrMagnitude == 0)
        {
            playerController.animator.SetFloat(moveXParameter, 0);
            playerController.animator.SetFloat(moveZParameter, 0);
            return;
        }
        UpdateMovementAnimation();

        float targetAngle = Mathf.Atan2(normInput.x, normInput.z) * Mathf.Rad2Deg + playerController.cameraTransform.eulerAngles.y;
        moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
        playerController.controller.Move(moveSpeed * Time.deltaTime * moveDir);
    }
    protected void UpdateMovementAnimation()
    {
        playerController.animator.SetFloat(moveXParameter, normInput.x);
        playerController.animator.SetFloat(moveZParameter, normInput.z);
    }
    protected void ApplyGravity()
    {
        gravityVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && gravityVelocity.y < 0.0f)
        {
            gravityVelocity.y = -1.0f;
        }
        if (!isGrounded)
        {
            playerController.controller.Move(gravityVelocity * Time.deltaTime);
        }

    }
}
