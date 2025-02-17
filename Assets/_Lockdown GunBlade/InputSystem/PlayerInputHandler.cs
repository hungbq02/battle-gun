using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;


public class PlayerInputHandler : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;
    public bool aim;
    public bool shoot;
    public bool reload;
    public bool sprint;
    public bool roll;



    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
    public bool cursorInputForLook = true;

    public void OnMove(InputValue value)
    {
        MoveInput(value.Get<Vector2>());
    }

    public void OnLook(InputValue value)
    {
       /* if (cursorInputForLook)
        {*/
            LookInput(value.Get<Vector2>());
      //  }
    }
    public void OnTouchZoneLookInput(Vector2 lookInput)
    {
        look = lookInput;
    }

    public void OnJump(InputValue value)
    {
        JumpInput(value.isPressed);
    }
    public void OnAim(InputValue value)
    {
        AimInput(value.isPressed);
    }
    public void OnShoot(InputValue value)
    {
        ShootInput(value.isPressed);
    }
    public void OnReload(InputValue value)
    {
        ReloadInput(value.isPressed);
    }
    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }

    public void OnRoll(InputValue value)
    {
        RollInput(value.isPressed);
    }


    ///
    public void MoveInput(Vector2 newMoveDirection)
    {
        move = newMoveDirection;
    }

    public void LookInput(Vector2 newLookDirection)
    {
        look = newLookDirection;
    }

    public void JumpInput(bool newJumpState)
    {
        jump = newJumpState;
    }
    public void AimInput(bool newAimInput)
    {
        aim = newAimInput;
    }
    public void ShootInput(bool newShootInput)
    {
        shoot = newShootInput;
    }
    public void ReloadInput(bool newReloadInput)
    {
        reload = newReloadInput;
    }
    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }
    public void RollInput(bool newRollState)
    {
        roll = newRollState;
    }



    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }
    //Fix Cursor can’t click in Locked mouse state
    //Cursor.lockState = CursorLockMode.None -> Cursor.lockState = CursorLockMode.Confined 
    public void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : Cursor.lockState = CursorLockMode.Confined; 
    }

}

