using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCoroutine : MonoBehaviour
{
    public IEnumerator CanShoot(RaycastWeapon weapon, float timeDelayShoot)
    {
        Debug.Log(weapon.gameObject.name + " CanShoot Coroutine started");
        weapon.canShoot = false;
        yield return new WaitForSeconds(timeDelayShoot);
        weapon.canShoot = true;
    }

    public IEnumerator Reload(RaycastWeapon weapon, PlayerController playerController, Animator animator, float reloadTime)
    {
        Debug.Log(weapon.gameObject.name + " Reload Coroutine started");
        weapon.isReloading = true;
        // AudioManager.instance.Play("Reload");
        playerController.SetAnimLayer("UpperBodyLayer", 1f);
        animator.SetBool("isReloading", true);
        yield return new WaitForSeconds(reloadTime);
        animator.SetBool("isReloading", false);
        if (weapon.magazineAmmo >= weapon.maxAmmo)
        {
            weapon.currentAmmo = weapon.maxAmmo;
            weapon.magazineAmmo -= weapon.maxAmmo;
        }
        else
        {
            weapon.currentAmmo = weapon.magazineAmmo;
            weapon.magazineAmmo = 0;
        }
        weapon.isReloading = false;
    }
}
