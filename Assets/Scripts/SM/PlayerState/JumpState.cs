using UnityEngine;

public class JumpState : BaseState
{
    bool isGrounded;
    float gravity;
    float jumpHeight;
    float playerSpeed;
    Vector3 airVelocity;




    public JumpState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        isGrounded = false;
        gravity = playerController.gravity;
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
        if (isGrounded)
        {
            stateMachine.ChangeState(playerController.landingState);
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
                            (airVelocity * playerController.airControl + velocity * (1 - playerController.airControl)) * playerSpeed * Time.deltaTime);
        }
        gravityVelocity.y += gravity * Time.deltaTime;
        isGrounded = playerController.isGrounded();
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
