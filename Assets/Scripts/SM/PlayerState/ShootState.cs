using System.Collections;
using UnityEngine;

public class ShootState : BaseState
{
    private bool canShoot = true;
    public ShootState(PlayerController _playerController, StateMachine _stateMachine) : base(_playerController, _stateMachine)
    {
        playerController = _playerController;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        playerController.animator.SetTrigger("shoot");
        playerController.input.shoot = false;


    }
    public override void UpdateLogic()
    {
        base.UpdateLogic();
        playerController.RotateTowardsCamera();

        if (playerController.input.shoot)
            Shoot();

        /*        if(!PlayerController.Instance.input.shoot)
                {
                    stateMachine.ChangeState(sm.idleState);
                }*/
    }
    public override void Exit()
    {
        base.Exit();
    }

    public void Shoot()
    {
        if (canShoot)
        {
            RaycastHit hit;
            GameObject bullet = GameObject.Instantiate(playerController.bulletPrefab, playerController.barrelTransform.position, Quaternion.identity, playerController.parentBullet.transform);
            BulletController bulletController = bullet.GetComponent<BulletController>();

            if (Physics.Raycast(playerController.cameraTransform.position, playerController.cameraTransform.forward, out hit, Mathf.Infinity))
            {
                bulletController.target = hit.point;
                bulletController.hit = true;
            }
            else
            {
                bulletController.target = playerController.cameraTransform.position + playerController.cameraTransform.forward * playerController.bulletHitMissDistance;
                bulletController.hit = false;
            }
            canShoot = false;
            playerController.StartCoroutine(CanShoot());
        }
    }

    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }

}
