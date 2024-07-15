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
            PlayerPrefs.SetInt("ID_LEVEL", 0);
            selectedLevelID = 0;
        }
        else
        {
            selectedLevelID = PlayerPrefs.GetInt("ID_LEVEL");
        }
        idLevel = transform.GetSiblingIndex();


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
