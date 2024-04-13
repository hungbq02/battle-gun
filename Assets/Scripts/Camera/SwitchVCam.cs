using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchVCam : MonoBehaviour
{
    public PlayerInputHandler input;
    public GameObject followPlayerCamera;
    public GameObject aimCamera;

    public bool isAiming = false;
    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
    }
    private void Update()
    {
        if (input.aim)
        {
            isAiming = !isAiming;
            input.aim = false;
            aimCamera.SetActive(isAiming);
            followPlayerCamera.SetActive(!isAiming);

            //Allow time for the camera to blend before enabling the UI
            //  StartCoroutine(ShowReticle());
            //   aimReticle.SetActive(false);
        }
    }
}
