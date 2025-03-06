using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public struct NavigationData
    {
        public string nameScreen;
        public object data;
    }
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject selectWeaponScreen;
    [SerializeField] private GameObject selectLevelScreen;
    [SerializeField] private GameObject settingScreen;
    [SerializeField] private GameObject exitScreen;

    private GameObject currentScreen;

    private Stack<NavigationData> navigationStack = new();
    private Dictionary<string, GameObject> screens = new();

    private void Start()
    {
        screens.Add("Home", homeScreen);
        screens.Add("SelectWeapon", selectWeaponScreen);
        screens.Add("SelectLevel", selectLevelScreen);
        screens.Add("Setting", settingScreen);
        screens.Add("Exit", exitScreen);

        NavigateTo("Home");
    }

    public void NavigateTo(string screenName, object data = null)
    {
        navigationStack.Push(new NavigationData { nameScreen = screenName, data = data });
        OnStackChanged();
    }

    public void NavigateBack()
    {
        if(navigationStack.Count > 1)
        {
            navigationStack.Pop();
            OnStackChanged();
        }
    }
    private void OnStackChanged()
    {
        if(navigationStack.Count > 0 )
        {
            var screenName = navigationStack.Peek().nameScreen;
            if(screens.ContainsKey(screenName))
            {
                if(currentScreen != null)
                {
                    currentScreen.SetActive(false);
                }
                currentScreen = screens[screenName];
                currentScreen.SetActive(true);
            }
        }
    }






}
