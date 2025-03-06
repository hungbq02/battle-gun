using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : ActionButton
{
    public override void OnButtonClicked()
    {
        screenManager.NavigateBack();
    }
}
