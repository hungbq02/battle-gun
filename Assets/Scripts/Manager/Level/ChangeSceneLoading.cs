using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneLoading : MonoBehaviour
{
    public void ChangeScene(int  idScene)
    {
        LevelManager.Instance.LoadScene(idScene);
    }
}
