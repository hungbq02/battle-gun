using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDialog : MonoBehaviour
{
    public GameObject[] newDialog;

    public void Switch(int idDialog)
    {
        if (newDialog != null)
        {
            gameObject.SetActive(false);
            newDialog[idDialog].SetActive(true);
        }
    }
    public void CheckDeactivePlayer(int idDialogCheck)
    {
        if(idDialogCheck != 3)
        {

        }
    }
}
