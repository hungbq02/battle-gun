using System.Collections;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    //tia lua khi ban
    public bool isShooting = false;
    public bool canShoot = true;
    public float timeDelayPistol;
    public ParticleSystem muzzleFlash;

    public GameObject bulletPrefab;
    public Transform barrelTransform;
    public Transform targetTransform;
    public float bulletHitMissDistance = 25f;

    Ray ray;
    RaycastHit hit;

/*    private void Update()
    {
        Debug.Log(isShooting);
    }*/
    public void StartShooting()
    {
        isShooting = true;
        if (!canShoot) return;

        muzzleFlash.Emit(1);
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
            bulletController.target = Camera.main.transform.position + Camera.main.transform.forward * bulletHitMissDistance;
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
        yield return new WaitForSeconds(timeDelayPistol);
        canShoot = true;
    }
}
