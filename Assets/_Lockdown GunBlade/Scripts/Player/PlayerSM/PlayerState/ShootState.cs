using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootState : Grounded
{
    public float transitionLayerSpeed = 15f;

    public ShootState(PlayerController _playerController, MovementSM _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        moveSpeed = playerController.shootMoveSpeed;
        isShooting = true;
    }
    public override void HandleInput()
    {
        base.HandleInput();
        HandleMovement();

    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();

        if (playerController.weapon.isReloading) return;

        if (playerController.weapon.readyToShoot)
        {
            if (playerController.input.shoot)
            {
                //If the mouse pointer is pointing at the UI, do not shoot
               /* if (IsPointerOverUIObject())
                {
                    return;
                }
*/
                playerController.weapon.StartShooting();

            }
            else
            {
                playerController.animator.SetFloat("ShootAnimSpeed", 1.0f); // reset speed animation shoot
                                                                            //  playerController.weapon.StopShooting();
                SmoothTransitionToLayer("UpperBodyLayer", 0f);

                /*//Get layer shooting
                float currentLayerWeight = playerController.animator.GetLayerWeight(1);

                //Smooth change layer
                playerController.SetAnimLayer("UpperBodyLayer", Mathf.Lerp(currentLayerWeight, 0f, Time.deltaTime * transitionLayerSpeed));

                if (currentLayerWeight < 0.001f)
                {
                    stateMachine.ChangeState(sm.idleState);
                }*/
            }
        }
        //Change state
        if (playerController.input.jump)
        {
            stateMachine.ChangeState(sm.jumpingState);
        }
    }
    public override void UpdatePhysics()
    {
        base.UpdatePhysics();
    }
    public override void Exit()
    {
        base.Exit();
        playerController.animator.Play("Shoot", -1, 0f);
        isShooting = false;
    }

    private void SmoothTransitionToLayer(string layerName, float targetWeight)
    {
        float currentLayerWeight = playerController.animator.GetLayerWeight(1);
        playerController.SetAnimLayer(layerName, Mathf.Lerp(currentLayerWeight, targetWeight, Time.deltaTime * transitionLayerSpeed));
        if (currentLayerWeight < 0.001f)
        {
            stateMachine.ChangeState(sm.idleState);
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
