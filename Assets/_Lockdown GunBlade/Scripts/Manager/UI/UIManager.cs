using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private BaseButton switchWeaponButton;
    [SerializeField] private WeaponSwiching weaponSwitching;

    private void Start()
    {
        switchWeaponButton.onClickAction = weaponSwitching.SwitchWeapon;
    }
}

