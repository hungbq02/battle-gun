using System.Collections;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    //tia lua khi ban
    public bool isShooting = false;
    private bool canShoot = true;
    public ParticleSystem muzzleFlash;

    public GameObject bulletPrefab;
    public Transform barrelTransform;
    public Transform targetTransform;
    public float bulletHitMissDistance = 25f;

    Ray ray;
    RaycastHit hit;

    private void Update()
    {
    }
    public void StartShooting()
    {
        isShooting = true;
        //  muzzleFlash.Emit(1);
        if (!canShoot) return;


        ray.origin = barrelTransform.position;
        ray.direction = targetTransform.position - barrelTransform.position;

        GameObject bullet = CreateBullet();
        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (Physics.Raycast(ray, out hit))
        {
            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
           // bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }
        canShoot = false;
        StartCoroutine(CanShoot());
    }
    public void StopShooting()
    {
        isShooting = false;
    }

    GameObject CreateBullet()
    {
        GameObject bullet = PoolManager.Instance.bulletPool.GetObject();
        bullet.transform.position = barrelTransform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.SetActive(true);
        return bullet;
    }
    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
    }
}
