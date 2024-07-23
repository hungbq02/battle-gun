using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchDialog : MonoBehaviour
{
    public GameObject[] newDialog;

    public void Switch(int idDialog)
    {
        Debug.Log("CLICK SWITCH BUTTON");
        if (newDialog != null)
        {
            gameObject.SetActive(false);
            newDialog[idDialog].SetActive(true);
        }
    }
}
