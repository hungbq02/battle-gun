using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaycastWeapon : MonoBehaviour
{
    [Header("Check")]
    public bool readyToShoot = true;
    public bool isReloading;

    [Header("Settings")]
    public ParticleSystem muzzleFlash;
    public Transform barrelTransform;
    public Transform camTransform;
    public Image reloadCircle;
    public LayerMask whatIsEnemy;
    public Text cooldownText;
    public Animator animator;
    public PlayerController playerController;
    public WeaponUI weaponUI;
    [SerializeField]
    private LineRenderer bulletTrail;

    [Header("Stats")]
    public int damage;
    public float spread; //Dispersion of bullets
    public float timeDelayShoot;
    public float range;
    public int currentAmmo;
    public int maxAmmo = 10;
    public int magazineAmmo = 30;
    public int bulletPerTap = 1;
    public float reloadTime = 1f;
    private float timeToDestroyBulletHole = 1f;
    private float timeToDestroyBulletTrail = 0.5f;


    Ray ray;
    RaycastHit hit;

    private float remainingTime;

    private void Start()
    {
        currentAmmo = maxAmmo;
        weaponUI.UpdateInfo(currentAmmo, magazineAmmo);
    }

    private void OnEnable()
    {
        isReloading = false;
        readyToShoot = true;

        animator.SetBool("isReloading", false);
        weaponUI.UpdateInfo(currentAmmo, magazineAmmo);
        UpdateReloadUI();
    }

    private void Update()
    {
        if (currentAmmo == 0 && magazineAmmo == 0 || isReloading) return;

        if (currentAmmo == maxAmmo)
        {
            playerController.input.reload = false;
            return;
        }
        if (CanReload())
        {
            StartCoroutine(Reload());
        }
    }
    //Check if the player can reload
    private bool CanReload()
    {
        return !isReloading && (currentAmmo == 0 && magazineAmmo > 0 || playerController.input.reload || currentAmmo < bulletPerTap);
    }
    public void StartShooting()
    {

        if (!readyToShoot || currentAmmo <= 0) return;

        PlayShootAnimation();

        //Update player rotation to match camera
        playerController.UpdatePlayerRotationToMatchCamera();

        for (int i = 0; i < bulletPerTap; i++)
        {
            muzzleFlash.Emit(1);
            AudioManager.Instance.PlaySFX("shooting");

            Vector3 direction = GetSpreadDirection();
            ray.origin = camTransform.position;
            ray.direction = direction;

            //Distance miss
            Vector3 hitPointMiss = camTransform.position + direction * range;

            if (Physics.Raycast(ray, out hit, range, whatIsEnemy))
            {
                SpawnBulletTrail(hit.point);
                HandleHit(hit);
            }
            else
            {
                SpawnBulletTrail(hitPointMiss);
            }
        }
        CinemachineShake.Instance.ShakeCamera(4, 0.1f);
        currentAmmo -= bulletPerTap;
        weaponUI.UpdateInfo(currentAmmo, magazineAmmo);
        readyToShoot = false;


        StartCoroutine(ReadyToShoot());
    }
    //Calculate the direction of the bullet with a certain spread
    private Vector3 GetSpreadDirection()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        return (camTransform.forward + new Vector3(x, y, 0f)).normalized;
    }
    private void HandleHit(RaycastHit hit)
    {
        GameObject hitObject = hit.collider.gameObject;

        //Check if the object is damageable
        if (hitObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
        }
        else
        {
            CreateBulletHole(hit.point, hit.normal);
        }
    }
    void CreateBulletHole(Vector3 position, Vector3 normal)
    {
        // Create bullet hole
        GameObject bulletHolePool = PoolManager.Instance.bulletHolePool.GetObject();
        if (bulletHolePool.activeSelf) return;

        bulletHolePool.transform.SetPositionAndRotation(position + normal * 0.01f, Quaternion.LookRotation(normal));
        bulletHolePool.SetActive(true);
        // Deactivate bullet hole after time
        StartCoroutine(DeactivateAfterTime(bulletHolePool, timeToDestroyBulletHole, PoolManager.Instance.bulletHolePool));
    }
    void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulletTrailEffect = PoolManager.Instance.bulletTrailPool.GetObject();
        LineRenderer lineRenderer = bulletTrailEffect.GetComponent<LineRenderer>();

        lineRenderer.SetPosition(0, barrelTransform.position);
        lineRenderer.SetPosition(1, hitPoint);
        bulletTrailEffect.SetActive(true);

        // Deactivate bullet trail after time
        StartCoroutine(DeactivateAfterTime(bulletTrailEffect, timeToDestroyBulletTrail, PoolManager.Instance.bulletTrailPool));
    }


    IEnumerator ReadyToShoot()
    {
        readyToShoot = false;
        yield return new WaitForSeconds(timeDelayShoot);
        readyToShoot = true;
    }

    //Reload
    IEnumerator Reload()
    {
        isReloading = true;
        AudioManager.Instance.PlaySFX("reload");

        playerController.SetAnimLayer("UpperBodyLayer", 1f);
        animator.SetBool("isReloading", true);

        // Cooldown reload UI
        reloadCircle.gameObject.SetActive(true);
        remainingTime = reloadTime;
        reloadCircle.fillAmount = 1;
        cooldownText.text = reloadTime.ToString();
        reloadCircle.DOFillAmount(0, reloadTime);

        DOTween.To(() => remainingTime, x => remainingTime = x, 0f, reloadTime)
            .SetEase(Ease.Linear)
            .OnUpdate(() => cooldownText.text = Mathf.Ceil(remainingTime).ToString("F2"))
            .OnComplete(() => reloadCircle.gameObject.SetActive(false));

        yield return new WaitForSeconds(reloadTime);
        ReloadFinished();


    }
    //Reload finished
    private void ReloadFinished()
    {
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
    //Play shoot animation
    private void PlayShootAnimation()
    {
        playerController.SetAnimLayer("UpperBodyLayer", 1f);
        playerController.animator.SetFloat("ShootAnimSpeed", 2f);
        playerController.animator.CrossFade("Shoot", 0.1f, 1, 0f);
    }
    //Update UI reload
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
    private IEnumerator DeactivateAfterTime(GameObject obj, float delay, Pooler pool)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnObject(obj);
    }



}

