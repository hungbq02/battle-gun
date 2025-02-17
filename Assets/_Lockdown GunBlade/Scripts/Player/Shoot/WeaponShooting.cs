using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WeaponShooting : BaseMonoBehaviour
{
    [Header("Shooting Settings")]
    public ParticleSystem muzzleFlash;
    public Transform barrelTransform;
    public Transform camTransform;
    public LayerMask whatIsEnemy = (1 << 6) | (1 << 8); // 8: Enemy, 6 Ground

    [Header("Weapon Stats")]
    [HideInInspector] public int damage;
    [HideInInspector] public float spread;           // the dispersion of bullets
    [HideInInspector] public float timeDelayShoot;
    [HideInInspector] public float range;
    [HideInInspector] public int maxAmmo = 10;
    [HideInInspector] public int magazineAmmo = 30;
    [HideInInspector] public int bulletPerTap = 1;
    public int currentAmmo;

    private bool readyToShoot = true;

    [SerializeField] private PlayerController playerController;
    [SerializeField] private WeaponUI weaponUI;

    private Ray ray;
    private RaycastHit hit;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerController();
        LoadWeaponUI();
        LoadParticle();
        LoadCamera();
        LoadBarrel();
    }
    /// <summary>
    /// Initialize ammo 
    /// </summary>
    public void InitializeAmmo()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoUI();
    }

    /// <summary>
    /// Update ammo UI
    /// </summary>


    public bool CanShoot()
    {
        return readyToShoot && currentAmmo > 0;
    }

    /// <summary>
    /// Check if the weaponShooting is out of ammo.
    /// </summary>
    public bool IsOutOfAmmo()
    {
        return currentAmmo == 0 && magazineAmmo == 0;
    }

    /// <summary>
    /// Check if the weaponShooting is full ammo.
    /// </summary>
    public bool IsFullAmmo()
    {
        return currentAmmo == maxAmmo;
    }

    /// <summary>
    /// process shooting
    /// </summary>
    public void PerformShooting()
    {
        PlayShootAnimation();

        for (int i = 0; i < bulletPerTap; i++)
        {
            ShootSingleBullet();
        }

        // Shake camera 
        CinemachineShake.Instance.ShakeCamera(4, 0.1f);

        // Update ammo
        currentAmmo -= bulletPerTap;
        UpdateAmmoUI();

        readyToShoot = false;
        StartCoroutine(ResetShot());
    }

    private void ShootSingleBullet()
    {
        muzzleFlash.Emit(1);
        AudioManager.Instance.PlaySFX("shooting");

        Vector3 direction = GetSpreadDirection();
        ray.origin = camTransform.position;
        ray.direction = direction;

        // Calculate miss distance
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

    /// <summary>
    /// Calculate the direction of the bullet spread.
    /// </summary>
    private Vector3 GetSpreadDirection()
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        return (camTransform.forward + new Vector3(x, y, 0f)).normalized;
    }

    /// <summary>
    /// hit the target
    /// </summary>
    private void HandleHit(RaycastHit hit)
    {
        GameObject hitObject = hit.collider.gameObject;
        if (hitObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
        }
        else
        {
            CreateBulletHole(hit.point, hit.normal);
        }
    }

    private void CreateBulletHole(Vector3 position, Vector3 normal)
    {
        GameObject bulletHole = PoolManager.Instance.bulletHolePool.GetObject();
        if (bulletHole.activeSelf) return;

        bulletHole.transform.SetPositionAndRotation(position + normal * 0.01f, Quaternion.LookRotation(normal));
        bulletHole.SetActive(true);
        StartCoroutine(DeactivateAfterTime(bulletHole, 1f, PoolManager.Instance.bulletHolePool));
    }

    private void SpawnBulletTrail(Vector3 hitPoint)
    {
        GameObject bulletTrailEffect = PoolManager.Instance.bulletTrailPool.GetObject();
        LineRenderer lineRenderer = bulletTrailEffect.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, barrelTransform.position);
        lineRenderer.SetPosition(1, hitPoint);
        bulletTrailEffect.SetActive(true);
        StartCoroutine(DeactivateAfterTime(bulletTrailEffect, 0.5f, PoolManager.Instance.bulletTrailPool));
    }

    private IEnumerator DeactivateAfterTime(GameObject obj, float delay, Pooler pool)
    {
        yield return new WaitForSeconds(delay);
        pool.ReturnObject(obj);
    }

    private IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(timeDelayShoot);
        readyToShoot = true;
    }

    private void PlayShootAnimation()
    {
        playerController.SetAnimLayer("UpperBodyLayer", 1f);
        playerController.animator.SetFloat("ShootAnimSpeed", 2f);
        playerController.animator.CrossFade("Shoot", 0.1f, 1, 0f);
    }

    internal void InitializeStats(int damage, float spread, float timeDelayShoot, float range, int maxAmmo, int magazineAmmo, int bulletPerTap)
    {
        this.damage = damage;
        this.spread = spread;
        this.timeDelayShoot = timeDelayShoot;
        this.range = range;
        this.maxAmmo = maxAmmo;
        this.magazineAmmo = magazineAmmo;
        this.bulletPerTap = bulletPerTap;
    }
    public void UpdateAmmoUI()
    {
        weaponUI.UpdateAmmoDisplay(currentAmmo, magazineAmmo);
    }
    protected virtual void LoadPlayerController()
    {
        if (playerController != null) return;
        playerController = transform.GetComponentInParent<PlayerController>();
    }
    protected virtual void LoadWeaponUI()
    {
        if (weaponUI != null) return;
        weaponUI = FindObjectOfType<WeaponUI>();
    }
    protected virtual void LoadParticle()
    {
        if (muzzleFlash != null) return;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
    }
    protected virtual void LoadCamera()
    {
        if (camTransform != null) return;
        camTransform = Camera.main.transform;
    }
    protected virtual void LoadBarrel()
    {
        if (barrelTransform != null) return;
        barrelTransform = transform.Find("Barrel");
    }
}
