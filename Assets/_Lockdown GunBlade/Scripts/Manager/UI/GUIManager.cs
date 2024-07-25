using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIManager : MonoBehaviour
{
    public GameObject loseDialog;
    private void Update()
    {
        if (loseDialog == null) return;
        if (!HealthSystemPlayer.isAlive)
        {
            loseDialog.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public static bool isPauseGame;
    public void Pause()
    {
        Time.timeScale = 0f;
        isPauseGame = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPauseGame = false;
    }
    public void MainMenu()
    {
        FindObjectOfType<ChangeSceneLoading>().BackToMainMenu();
    }
    public void Replay()
    {
        Time.timeScale = 1;     
        FindObjectOfType<ChangeSceneLoading>().ReplayGame();
    }
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
