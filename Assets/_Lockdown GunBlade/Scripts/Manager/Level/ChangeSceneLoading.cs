using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneLoading : MonoBehaviour
{
    public void PlayLevel()
    {
        int idScene = PlayerPrefs.GetInt("ID_LEVEL");
        SceneLoading.Instance.LoadScene(idScene);
    }
}
