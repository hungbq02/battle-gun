using UnityEngine;

public class PlayerCollision : BaseMonoBehaviour
{
    private WeaponShooting weaponShooting;
    [SerializeField] private HealthSystemPlayer healthPlayer;
    [SerializeField] private WeaponUI weaponUI;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHealthSystemPlayer();
        LoadWeaponUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (weaponShooting == null)
        {
            LoadWeaponShooting();
        }

        if (other.gameObject.CompareTag("HealItem"))
        {
            healthPlayer.Heal(200);
        }
        else if (other.gameObject.CompareTag("BulletItem"))
        {
            int ammoToAdd = 20;

            weaponShooting.currentAmmo += ammoToAdd;

            if (weaponShooting.currentAmmo > weaponShooting.maxAmmo)
            {
                int excessAmmo = weaponShooting.currentAmmo - weaponShooting.maxAmmo;
                weaponShooting.currentAmmo = weaponShooting.maxAmmo;
                weaponShooting.magazineAmmo += excessAmmo;
            }
            //update UI
            weaponUI.UpdateAmmoDisplay(weaponShooting.currentAmmo, weaponShooting.magazineAmmo);
        }
    }
    //Load Components
    protected virtual void LoadHealthSystemPlayer()
    {
        if (healthPlayer != null) return;
        healthPlayer = GetComponent<HealthSystemPlayer>();
    }
    protected virtual void LoadWeaponShooting()
    {

        if (weaponShooting != null) return;
        weaponShooting = transform.GetComponentInChildren<WeaponShooting>();
    }
    protected virtual void LoadWeaponUI()
    {
        if (weaponUI != null) return;
        weaponUI = FindObjectOfType<WeaponUI>();
    }

}
