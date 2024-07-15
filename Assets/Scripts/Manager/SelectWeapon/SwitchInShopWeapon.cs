using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchInShopWeapon : MonoBehaviour
{
    public int selectWeapon = 0;
    public GameObject riffle;
    public GameObject shotgun;


    private void Start()
    {
        SelectWeapon();
    }
    public void ChangeWeapon()
    {
        int previousSelectedWeapon = selectWeapon;

        if (selectWeapon >= transform.childCount - 1)
            selectWeapon = 0;
        else
            selectWeapon++;


        if (previousSelectedWeapon != selectWeapon)
        {
            SelectWeapon();
        }
    }
    private void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
