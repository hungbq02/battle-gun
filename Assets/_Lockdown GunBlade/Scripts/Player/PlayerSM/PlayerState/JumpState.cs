using UnityEngine;

public class JumpState : BaseState
{
    bool isGrounded;
    float jumpHeight;
    float playerSpeed;
    Vector3 airVelocity;

    private MovementSM sm;
    public JumpState(PlayerController playerController, MovementSM stateMachine) : base(playerController, stateMachine)
    {
        sm = (MovementSM)this.stateMachine;
    }

    public override void Enter()
    {
        //base.Enter();
        gravity = playerController.gravity;
        isGrounded = false;
        jumpHeight = playerController.jumpHeight;
        playerSpeed = playerController.moveSpeed;
        gravityVelocity.y = 0;

        playerController.animator.SetFloat("MoveX", 0);
        playerController.animator.SetFloat("MoveZ", 0);

        playerController.animator.SetTrigger("jump");
        Jump();
    }
    public override void UpdateLogic()
    {
        //Control player while in the air
        playerController.airControl = playerController.input.sprint ? 1.5f : 0.5f;
        if (isGrounded)
        {
            stateMachine.ChangeState(sm.landingState);
        }
        else
        {
            velocity = playerController.playerVelocity;
            airVelocity = new Vector3(playerController.input.move.x, 0, playerController.input.move.y);

            velocity = velocity.x * playerController.cameraTransform.right.normalized + velocity.z * playerController.cameraTransform.forward.normalized;
            velocity.y = 0;
            airVelocity = airVelocity.x * playerController.cameraTransform.right.normalized + airVelocity.z * playerController.cameraTransform.forward.normalized;
            airVelocity.y = 0;
            playerController.controller.Move(gravityVelocity * Time.deltaTime +
                            playerSpeed * Time.deltaTime * (airVelocity * playerController.airControl + velocity * (1 - playerController.airControl)));
        }
        gravityVelocity.y += gravity * Time.deltaTime;
        isGrounded = playerController.IsGrounded();
    }

    public override void Exit()
    {
        playerController.input.jump = false;
    }

    private void Jump()
    {
        gravityVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

}