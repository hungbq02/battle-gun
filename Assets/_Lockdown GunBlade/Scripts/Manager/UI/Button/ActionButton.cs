using UnityEngine;
using UnityEngine.UI;

public abstract class ActionButton : MonoBehaviour
{
    private Button btn;
    protected ScreenManager screenManager;
    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClicked);
        screenManager = FindObjectOfType<ScreenManager>();
    }

    public abstract void OnButtonClicked();
}
