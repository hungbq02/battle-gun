using UnityEngine;

public class ChangeSceneLoading : MonoBehaviour
{
    public void PlayLevel()
    {
        int idScene = PlayerPrefs.GetInt("ID_LEVEL");
        Time.timeScale = 1f;
        SceneLoading.Instance.LoadScene(idScene);
    }
    public void BackToMainMenu()
    {
        SceneLoading.Instance.LoadScene(0);
    }
}
