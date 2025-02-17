using System;
using UnityEngine;
using UnityEngine.UI;

public class BaseButton : MonoBehaviour
{
    [Header("Base Button")]
    [SerializeField] protected Button button;
    public Action onClickAction;

    protected virtual void Awake()
    {
        LoadButton();
    }

    protected virtual void Start()
    {
        AddOnClickEvent();
    }

    protected void LoadButton()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
            if (button == null)
            {
                Debug.LogError(transform.name + ": ", gameObject);
            }
        }
    }

    protected void AddOnClickEvent()
    {
        if (button != null)
        {
            button.onClick.AddListener(OnClickInternal);
        }
    }

    private void OnClickInternal()
    {
        onClickAction?.Invoke(); // Call Action
        OnClick();
    }

    protected virtual void OnClick()
    {
        Debug.Log(transform.name + " called");
    }
}
