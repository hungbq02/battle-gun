﻿using UnityEngine;
using UnityEngine.UI;

public class WeaponSwiching : MonoBehaviour
{
    public int selectedWeapon = 0; // <=> The number of the parent's child object
    public PlayerController playerController;
    public Animator animPlayer; 
    public Image weaponIcon;
    public WeaponUI weaponUI;

    public AnimatorOverrideController pistolAnimatorOverride;
    public AnimatorOverrideController rifleAnimatorOverride;
    public AnimatorOverrideController shotgunAnimatorOverride;

    private void Awake()
    {
        SelectWeapon();
    }
    private void Update()
    {
        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <=0)
                selectedWeapon = transform.childCount -1;
            else
                selectedWeapon--;
        }

        if(previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }    
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

        //Update anim
        switch (selectedWeapon)
        {
            case 0: //Pistol
                animPlayer.runtimeAnimatorController = pistolAnimatorOverride;
                break;
            case 1: // Rifle
                animPlayer.runtimeAnimatorController = rifleAnimatorOverride;
                break;
            case 2: // Shotgun
                animPlayer.runtimeAnimatorController = shotgunAnimatorOverride;
                break;
            default:
                animPlayer.runtimeAnimatorController = pistolAnimatorOverride; // Mặc định là Pistol
                break;
        }

        //Update component weapon
        playerController.weapon = transform.GetChild(selectedWeapon).GetComponent<RaycastWeapon>();

        //Update WeaponUI
        weaponIcon.sprite = weaponUI.listIconWeapon[selectedWeapon];
    }
}
