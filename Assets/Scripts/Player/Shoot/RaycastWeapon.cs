using System.Collections;
using UnityEngine;

public class RaycastWeapon : MonoBehaviour
{
    //tia lua khi ban
    public bool isShooting = false;
    public bool canShoot = true;
    public float timeDelayShoot;
    public ParticleSystem muzzleFlash;

  //  public GameObject bulletPrefab;
    public Transform barrelTransform;
    public Transform targetTransform;
    public float bulletHitMissDistance = 25f;


    public Animator animator;
    public PlayerController playerController;

    public int currentAmmo;
    public int maxAmmo = 10;
    public int magazineAmmo = 30;

    public float reloadTime = 1f;
    public bool isReloading;

    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        currentAmmo = maxAmmo;

    }
    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool("isReloading", false);
    }
    private void Update()
    {
       // Debug.Log("timedelay " + timeDelayShoot);
        if (currentAmmo == 0 && magazineAmmo == 0)
        {
            return;
        }

        if (isReloading)
            return;
        if (currentAmmo == 0 && magazineAmmo > 0 && !isReloading)
        {
            playerController.SetAnimLayer("UpperBodyLayer", 1f);
            StartCoroutine(Reload());
        }
    }
    public void StartShooting()
    {
        Debug.Log(playerController.weapon.name);
        isShooting = true;
        if (!canShoot || currentAmmo <= 0) return;

        muzzleFlash.Emit(1);
        currentAmmo--;
        Debug.Log("Ammo" + currentAmmo);


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
        yield return new WaitForSeconds(timeDelayShoot);
        canShoot = true;
    }
    IEnumerator Reload()
    {
        isReloading = true;
        //AudioManager.instance.Play("Reload");
        playerController.SetAnimLayer("UpperBodyLayer", 1f);
        animator.SetBool("isReloading", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("isReloading", false);
        if (magazineAmmo >= maxAmmo)
        {
            currentAmmo = maxAmmo;
            magazineAmmo -= maxAmmo;
        }
        else
        {
            currentAmmo = magazineAmmo;
            magazineAmmo = 0;
        }
        isReloading = false;
    }

    
}
