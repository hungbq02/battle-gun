using UnityEngine;

[CreateAssetMenu(menuName = "Gun/NewGunStats", fileName = "NewGunStats")]
public class GunStatsSO: ScriptableObject
{
    public int damage;
    public float spread;
    public float timeDelayShoot;
    public float range;
    public int maxAmmo;
    public int magazineAmmo;
    public int bulletPerTap;
    public float reloadTime;
}
