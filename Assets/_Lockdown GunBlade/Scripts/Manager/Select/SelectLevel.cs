using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : MonoBehaviour
{
    public static List<SelectLevel> listSelected = new List<SelectLevel>();
    [SerializeField] GameObject selectedImg;
    int idLevel;
    int selectedLevelID;
    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("ID_LEVEL"))
        {
            PlayerPrefs.SetInt("ID_LEVEL", 1);
            selectedLevelID = 1;
        }
        else
        {
            selectedLevelID = PlayerPrefs.GetInt("ID_LEVEL");
        }
        //GetSiblingIndex() from 0 ->
        idLevel = transform.GetSiblingIndex() + 1; // Level from 1 to 3


        if (idLevel == selectedLevelID)
        {
            listSelected.Add(this);
            ShowSelected();
        }
        else
        {
            HideSelected();
        }

    }
    public void ClickLevel()
    {
        Debug.Log(idLevel);
        //Xoa tich TickSelectedMode truoc do
        foreach (SelectLevel selected in listSelected)
        {
            selected.HideSelected();
        }
        ShowSelected();
        listSelected.Add(this);
        PlayerPrefs.SetInt("ID_LEVEL", idLevel);

    }
    private void ShowSelected()
    {
        selectedImg.SetActive(true);

    }
    public void HideSelected()
    {
        selectedImg.SetActive(false);

    }
}
