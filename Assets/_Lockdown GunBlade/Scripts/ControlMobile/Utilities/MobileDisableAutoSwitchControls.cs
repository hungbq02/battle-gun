using UnityEngine;
using UnityEngine.InputSystem;

public class MobileDisableAutoSwitchControls : MonoBehaviour
{
    
#if ENABLE_INPUT_SYSTEM && (UNITY_IOS || UNITY_ANDROID)

    [Header("Target")]
    public PlayerInput playerInput;

    void Start()
    {
        DisableAutoSwitchControls();
    }

    void DisableAutoSwitchControls()
    {
        playerInput.neverAutoSwitchControlSchemes = true;
    }

#endif
    
}
