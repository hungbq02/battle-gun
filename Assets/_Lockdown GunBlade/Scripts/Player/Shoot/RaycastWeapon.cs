using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class RaycastWeapon : MonoBehaviour
{
    //Weapon ID :   Pistol     0
    //              Rifle      1
    //              Shotgun    2
    ///
    //tia lua khi ban
    public bool isShooting = false;
    public bool canShoot = true;
    public bool isReloading;

    public float timeDelayShoot;
    public ParticleSystem muzzleFlash;

    //  public GameObject bulletPrefab;
    public Transform barrelTransform;
    public Transform targetTransform;
    public float bulletHitMissDistance = 25f;
    public Image reloadCircle;


    public Animator animator;
    public PlayerController playerController;
    public WeaponUI weaponUI;

    public int currentAmmo;
    public int maxAmmo = 10;
    public int magazineAmmo = 30;

    public float reloadTime = 1f;
    private float remainingTime;
    public Text cooldownText;

    Ray ray;
    RaycastHit hit;

    private void Start()
    {
        currentAmmo = maxAmmo;
        weaponUI.UpdateInfo(currentAmmo, magazineAmmo);

    }
    private void OnEnable()
    {
        isReloading = false;
        isShooting = false;
        canShoot = true;

        animator.SetBool("isReloading", false);
        weaponUI.UpdateInfo(currentAmmo, magazineAmmo);
        UpdateReloadUI();
    }
    private void Update()
    {
        if (currentAmmo == 0 && magazineAmmo == 0)
        {
            return;
        }

        if (currentAmmo == maxAmmo)
        {
            playerController.input.reload = false;
            return;
        }

        if (isReloading)
            return;
        if (currentAmmo == 0 && magazineAmmo > 0 || playerController.input.reload && !isReloading)
        {
            Debug.Log("reload");
            playerController.SetAnimLayer("UpperBodyLayer", 1f);
            StartCoroutine(Reload());
        }
    }
    //Fire
    public void StartShooting()
    {
        // Debug.Log(playerController.weapon.name);

        isShooting = true;
        if (!canShoot || currentAmmo <= 0) return;

        muzzleFlash.Emit(1);
        AudioManager.Instance.PlaySFX("shooting");
        currentAmmo--;
        weaponUI.UpdateInfo(currentAmmo, magazineAmmo);


        ray.origin = barrelTransform.position;
        ray.direction = targetTransform.position - barrelTransform.position;

        GameObject bullet = CreateBullet();
        BulletController bulletController = bullet.GetComponent<BulletController>();

        bullet.transform.forward = ray.direction.normalized;

        float distanceCheck = Vector3.Distance(barrelTransform.position, targetTransform.position);
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
        // If weapon is shotgun, check distance
        if (this.gameObject.name == "ShotGun")
        {
            if (distanceCheck > 15f)
            {
                bulletController.hit = false;
            }
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
        //Get current weapon ID 
        int typeWeapon = WeaponSwiching.selectedWeapon;
        //Get the correct bullet pool based on the current weapon type
        Pooler bulletPool = PoolManager.Instance.GetBulletPool(typeWeapon);

        GameObject bullet = bulletPool.GetObject();
        bullet.transform.SetPositionAndRotation(barrelTransform.position, Quaternion.identity);
        bullet.SetActive(true);
        return bullet;
    }
    IEnumerator CanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(timeDelayShoot);
        isShooting = false;
        canShoot = true;
    }

    IEnumerator Reload()
    {
        isReloading = true;
        AudioManager.Instance.PlaySFX("reload");

        playerController.SetAnimLayer("UpperBodyLayer", 1f);
        animator.SetBool("isReloading", true);

        //Cooldown reload UI
        reloadCircle.gameObject.SetActive(true);
        remainingTime = reloadTime;
        reloadCircle.fillAmount = 1;
        cooldownText.text = reloadTime.ToString();
        reloadCircle.DOFillAmount(0, reloadTime);

        DOTween.To(() => remainingTime, x => remainingTime = x, 0f, reloadTime)
            .SetEase(Ease.Linear)
            .OnUpdate(() => cooldownText.text = Mathf.Ceil(remainingTime).ToString("F2"))
            .OnComplete(() => reloadCircle.gameObject.SetActive(false)); //Deactive cooldown UI

        //wait reload
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
        weaponUI.UpdateInfo(currentAmmo, magazineAmmo);


        isReloading = false;
        playerController.input.reload = false;
    }
    private void UpdateReloadUI()
    {
        if (isReloading)
        {
            reloadCircle.gameObject.SetActive(true);
            reloadCircle.fillAmount = remainingTime / reloadTime;
            cooldownText.text = Mathf.Ceil(remainingTime).ToString("F2");
        }
        else
        {
            reloadCircle.gameObject.SetActive(false);
        }
    }



}

