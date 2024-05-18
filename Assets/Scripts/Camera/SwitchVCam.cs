using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchVCam : MonoBehaviour
{
    public PlayerInputHandler input;
    public GameObject thirdPersonCamera;
    public GameObject aimCamera;

    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    private PlayerController playerController;



    public bool isAiming = false;
    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        if (input.aim)
        {
            aimCamera.SetActive(true);
            thirdPersonCamera.SetActive(false);

            playerController.SetSensitivity(aimSensitivity);
        }
        else
        {
            aimCamera.SetActive(false);
            thirdPersonCamera.SetActive(true);

            playerController.SetSensitivity(normalSensitivity);
        }    
    }
}
