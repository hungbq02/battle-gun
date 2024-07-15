using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private GameObject canvasLoading;
    [SerializeField] private Image processBar;
    private float target;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public async void LoadScene(int idScene)
    {
        target = 0;
        processBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(idScene);
        scene.allowSceneActivation = false;

        canvasLoading.SetActive(true);

        do
        {
            await Task.Delay(100);
            target = scene.progress;
        } while (scene.progress < 0.9f);

        scene.allowSceneActivation = true;
        canvasLoading.SetActive(false);
    }
    public void Update()
    {
        processBar.fillAmount = Mathf.MoveTowards(processBar.fillAmount, target, 3 * Time.deltaTime);
    }

}
