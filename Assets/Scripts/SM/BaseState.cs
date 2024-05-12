using UnityEngine;

public class BaseState
{
    public PlayerController playerController;

    protected StateMachine stateMachine;
    protected Vector3 velocity;
    protected Vector3 gravityVelocity;

    float gravity;
    Vector3 moveDir = Vector3.zero;
    float playerSpeed;
    bool isGrounded;
    public BaseState(PlayerController playerController, StateMachine stateMachine)
    {
        this.playerController = playerController;
        this.stateMachine = stateMachine;
    }
    public virtual void Enter(){
        playerSpeed = playerController.MoveSpeed;
        isGrounded = playerController.isGrounded();
        gravity = playerController.gravity;
    }
    public virtual void HandleInput()
    {
        velocity = new Vector3(playerController.input.move.x, 0.0f, playerController.input.move.y).normalized;
        if (velocity.sqrMagnitude == 0f) return;
        float targetAngle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg + playerController.cameraTransform.eulerAngles.y;

        moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        velocity = moveDir;
    }
    public virtual void UpdateLogic()
    {
        gravityVelocity.y += gravity * Time.deltaTime;
        isGrounded = playerController.isGrounded();

        if (isGrounded && gravityVelocity.y < 0.0f)
        {
            gravityVelocity.y = -1.0f;
        }
        playerController.controller.Move(velocity * Time.deltaTime * playerSpeed + gravityVelocity * Time.deltaTime);
    }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}
