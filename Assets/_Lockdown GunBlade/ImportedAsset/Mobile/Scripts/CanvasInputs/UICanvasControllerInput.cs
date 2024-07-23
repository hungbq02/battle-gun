using UnityEngine;

public class UICanvasControllerInput : MonoBehaviour
{

    [Header("Output")]
    public PlayerInputHandler input;

    public void VirtualMoveInput(Vector2 virtualMoveDirection)
    {
        input.MoveInput(virtualMoveDirection);
    }

    public void VirtualLookInput(Vector2 virtualLookDirection)
    {
        input.LookInput(virtualLookDirection);
    }

    public void VirtualJumpInput(bool virtualJumpState)
    {
        input.JumpInput(virtualJumpState);
    }
    public void VirtualShootInput(bool virtualShootState)
    {
        input.ShootInput(virtualShootState);
    }
    public void VirtualAimInput(bool virtualAimState)
    {
        input.AimInput(virtualAimState);
    }

}

