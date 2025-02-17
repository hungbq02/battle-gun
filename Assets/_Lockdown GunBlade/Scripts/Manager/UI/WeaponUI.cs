using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Text txtCountBullet;
    public Sprite[] listIconWeapon;
    public Image weaponIcon;

    public void UpdateAmmoDisplay(int currentAmmo, int magazineAmmo)
    {
        txtCountBullet.text = $"{currentAmmo} / {magazineAmmo}";
    }
    public void UpdateWeaponIcon(int index)
    {
        weaponIcon.sprite = listIconWeapon[index];
    }
}
