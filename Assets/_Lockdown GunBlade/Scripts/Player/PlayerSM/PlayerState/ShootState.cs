using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShootState : BaseState
{
    int moveXParameter;
    int moveZParameter;


    public float transitionLayerSpeed = 14f;

    public ShootState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        moveXParameter = playerController.moveXAnimationParameterID;
        moveZParameter = playerController.moveZAnimationParameterID;

    }
    public override void HandleInput()
    {
        base.HandleInput();

        playerController.animator.SetFloat(moveXParameter, playerController.input.move.x);
        playerController.animator.SetFloat(moveZParameter, playerController.input.move.y);
    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
/*        if(!playerController.weapon.canShoot)
        {
            Debug.Log("CANSHOT");
        }*/
        if (playerController.weapon.isReloading)
        {
            Debug.Log("reloading");
        }
        if (playerController.weapon.canShoot && !playerController.weapon.isReloading)
        {
            if (playerController.input.shoot)
            {
                //If the mouse pointer is pointing at the UI, do not shoot
                if (IsPointerOverUIObject())
                {
                    Debug.Log("CLick UI");
                    return;
                }
                playerController.SetAnimLayer("UpperBodyLayer", 1f);
                playerController.animator.SetFloat("AnimationSpeed", 2.5f); //x3 speed animation shooting
                playerController.animator.CrossFade("Shoot", 0.1f, 1, 0f);
                playerController.weapon.StartShooting();

            }
            else
            {
                playerController.animator.SetFloat("AnimationSpeed", 1.0f); // reset speed animation shoot
                playerController.weapon.StopShooting();

                //Get layer shooting
                float currentLayerWeight = playerController.animator.GetLayerWeight(1);

                //Smooth change layer
                playerController.SetAnimLayer("UpperBodyLayer", Mathf.Lerp(currentLayerWeight, 0f, Time.deltaTime * transitionLayerSpeed));

                if (currentLayerWeight < 0.001f)
                {
                    playerController.movementSM.ChangeState(playerController.standingState);
                }
            }
        }

        if (playerController.weapon.isReloading)
        {
            stateMachine.ChangeState(playerController.reloadState);
        }

        //Change state
        if (playerController.input.jump)
        {
            stateMachine.ChangeState(playerController.jumpingState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        playerController.animator.Play("Shoot", -1, 0f);
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
