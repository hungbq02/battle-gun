using UnityEngine;

public class ChangeSceneLoading : MonoBehaviour
{
    public void PlayLevel()
    {
        int idScene = PlayerPrefs.GetInt("ID_LEVEL");

        SceneLoading.Instance.LoadScene(idScene);
    }
    public void BackToMainMenu()
    {
        SceneLoading.Instance.LoadScene(0);
    }

    internal void ReplayGame()
    {
        int idScene = PlayerPrefs.GetInt("ID_LEVEL");

        SceneLoading.Instance.LoadScene(idScene);
    }
}
