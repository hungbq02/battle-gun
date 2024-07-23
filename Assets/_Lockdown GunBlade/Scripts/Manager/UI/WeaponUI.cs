using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private Text txtCountBullet;
    public Sprite[] listIconWeapon;
    public void UpdateInfo(int currentAmmo, int magazineAmmo)
    {
        txtCountBullet.text = $"{currentAmmo} / {magazineAmmo}";
    }
}
