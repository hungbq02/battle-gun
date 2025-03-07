﻿using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public float moveSpeed = 4.5f;
    public float sprintSpeed = 6.5f;
    public float shootMoveSpeed = 3.5f;

    public float jumpHeight;
    public float airControl;


    #region Variables: Gravity & direction
    public float gravity = -9.81f;
    [HideInInspector] public Vector3 playerVelocity;
    [SerializeField] float gravityMultiplier = 2f;

    [HideInInspector] public int moveXAnimationParameterID;
    [HideInInspector] public int moveZAnimationParameterID;
    #endregion

    #region Variables: Ground

    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.28f;

    public LayerMask GroundLayers;
    #endregion

    #region Variables: Cinemachine

    [Header("Cinemachine")]
    public GameObject CinemachineCameraTarget;

    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;

    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;

    [Tooltip("For locking the camera position on all axis")]
    public bool LockCameraPosition = false;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    #endregion


    public float sensitivity = 1f;


    #region Variables: Player Control
    private PlayerInput _playerInput;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public PlayerInputHandler input;
    [HideInInspector] public UIVirtualTouchZone touchField;
    [HideInInspector] public UIVirtualJoystick moveJoystick;


    [HideInInspector] public Transform cameraTransform;
    #endregion

    [Header("Rotate Player Towards Camera")]
    public float rotationSmoothTime;
    float turnSmoothVelocity;
    private const float _threshold = 0.01f;


    [Header("Player Shoot")]
    public RaycastWeapon weapon;

    protected void Awake()
    {
        moveXAnimationParameterID = Animator.StringToHash("MoveX");
        moveZAnimationParameterID = Animator.StringToHash("MoveZ");
    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputHandler>();
        _playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<RaycastWeapon>();
        cameraTransform = Camera.main.transform;
        touchField = FindObjectOfType<UIVirtualTouchZone>();
        moveJoystick = FindObjectOfType<UIVirtualJoystick>();
        // input.SetCursorState(true);

        gravity *= gravityMultiplier;
    }
    private void Update()
    {
        if (!HealthSystemPlayer.isAlive) return;
        RotateTowardsCamera();

    }
    private void LateUpdate()
    {
        CameraRotation();
    }
    public bool IsGrounded()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        return Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }

    private void CameraRotation()
    { 
        // if there is an inputDir and camera position is not fixed
        if (input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse inputDir by Time.deltaTime;

            _cinemachineTargetYaw += input.look.x * sensitivity;
            _cinemachineTargetPitch += input.look.y * sensitivity;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }

    public void RotateTowardsCamera()
    {
        // Get the angle between the character's current forward direction and the camera's forward direction
        float targetAngle = Mathf.Atan2(cameraTransform.forward.x, cameraTransform.forward.z) * Mathf.Rad2Deg;

        // Smoothly rotate the character towards the camera's forward direction
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
    public void UpdatePlayerRotationToMatchCamera()
    {
        // Get the angle the camera
        float targetAngle = Mathf.Atan2(cameraTransform.forward.x, cameraTransform.forward.z) * Mathf.Rad2Deg;

        // rotate the character towards the camera's direction
        transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    public void SetSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }
    public void SetAnimLayer(string nameLayer, float weight)
    {
        animator.SetLayerWeight(animator.GetLayerIndex(nameLayer), weight);
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (IsGrounded()) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }



}

