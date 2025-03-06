using UnityEngine;
using UnityEngine.UI;

public class WeaponSwiching: BaseMonoBehaviour
{
    private int selectedWeapon = 0; // <=> The number of the parent's child object
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animPlayer;

    [SerializeField] private WeaponAnimatorSO DataWeaponAnimator;

    private AnimatorOverrideController pistolAnimatorOverride;
    private AnimatorOverrideController rifleAnimatorOverride;
    private AnimatorOverrideController shotgunAnimatorOverride;

    [SerializeField] private WeaponUI weaponUI;
    protected override void Awake()
    {
        base.Awake();
        SelectWeapon();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadPlayerController();
        LoadAnimPlayer();
        LoadWeaponUI();

        LoadPistolAnimatorOverride();
        LoadRifleAnimatorOverride();
        LoadShotgunAnimatorOverride();
    }
    public void SwitchWeapon()
    {
        selectedWeapon = (selectedWeapon + 1) % transform.childCount;
        SelectWeapon();       
        /*        selectedWeapon++;
                if (selectedWeapon >= transform.childCount)
                    selectedWeapon = 0;*/
    }
    private void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }

        //Update anim player
        animPlayer.runtimeAnimatorController = selectedWeapon switch
        {
            //Pistol
            0 => pistolAnimatorOverride,
            // Rifle
            1 => rifleAnimatorOverride,
            // Shotgun
            2 => shotgunAnimatorOverride,
            _ => pistolAnimatorOverride,// Mặc định là Pistol
        };

        //Update component weaponShooting
        playerController.weapon = transform.GetChild(selectedWeapon).GetComponent<RaycastWeapon>();

        //Update WeaponUI
        weaponUI.UpdateWeaponIcon(selectedWeapon);
    }
    //Load components
    protected virtual void LoadPlayerController()
    {
        if (playerController != null) return;
        playerController = GetComponentInParent<PlayerController>();
    }
    protected virtual void LoadAnimPlayer()
    {
        if (animPlayer != null) return;
        animPlayer = GetComponentInParent<Animator>();
    }
    protected virtual void LoadWeaponUI()
    {
        if (weaponUI != null) return;
        weaponUI = FindObjectOfType<WeaponUI>();
    }
    protected virtual void LoadPistolAnimatorOverride()
    {
        if (pistolAnimatorOverride != null) return;
        pistolAnimatorOverride = DataWeaponAnimator.pistolAnimator;
    }
    protected virtual void LoadRifleAnimatorOverride()
    {
        if (rifleAnimatorOverride != null) return;
        rifleAnimatorOverride = DataWeaponAnimator.rifleAnimator;
    }
    protected virtual void LoadShotgunAnimatorOverride()
    {
        if (shotgunAnimatorOverride != null) return;
        shotgunAnimatorOverride = DataWeaponAnimator.shotgunAnimator;
    }
}
