using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIManager : Singleton<GUIManager>
{
    public GameObject loseDialog;
    public GameObject winDialog;
    PlayerController player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }
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
        player.LockCameraPosition = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPauseGame = false;
        player.LockCameraPosition = false;
    }
    public void MainMenu()
    {
        FindObjectOfType<ChangeSceneLoading>().BackToMainMenu();
    }
    public void Replay()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(currentScene.name);
    }
    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
    public void ShowWinDialog()
    {
        StartCoroutine(DelayShowWinDialog());
    }

    IEnumerator DelayShowWinDialog()
    {
        yield return new WaitForSeconds(2.5f);
        isPauseGame = true;
        winDialog.SetActive(true);

    }
}
