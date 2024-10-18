using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    RaycastWeapon weapon;
    HealthSystemPlayer healthPlayer;
    private void Start()
    {
        healthPlayer = GetComponent<HealthSystemPlayer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        weapon = GetComponentInChildren<RaycastWeapon>();

        if (other.gameObject.CompareTag("HealItem"))
        {
            healthPlayer.Heal(200);
        }
        if (other.gameObject.CompareTag("BulletItem"))
        {
            int ammoToAdd = 20;

            weapon.currentAmmo += ammoToAdd;

            if (weapon.currentAmmo > weapon.maxAmmo)
            {
                int excessAmmo = weapon.currentAmmo - weapon.maxAmmo;
                weapon.currentAmmo = weapon.maxAmmo;
                weapon.magazineAmmo += excessAmmo;
            }
            weapon.weaponUI.UpdateInfo(weapon.currentAmmo, weapon.magazineAmmo);
        }
    }
}
