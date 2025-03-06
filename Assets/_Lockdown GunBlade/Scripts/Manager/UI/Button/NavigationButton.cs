using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationButton : ActionButton
{
    [SerializeField] private string sceneName;
    public override void OnButtonClicked()
    {
        screenManager.NavigateTo(sceneName);
    }
}
