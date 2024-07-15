using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int maxLevel = 2;
    public int currentLevel;
    public int currentWeapon;

    private void Awake()
    {
        currentLevel =  PlayerPrefs.GetInt("CURRENT_LEVEL");
        currentWeapon = PlayerPrefs.GetInt("CURRENT_WEAPON");
    }
    public void PlayLevel()
    {
        int idScene = PlayerPrefs.GetInt("ID_LEVEL");
        SceneManager.LoadScene(idScene+1);
    }
}
