using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Player")]
    public float moveSpeed = 4.5f;
    public float sprintSpeed = 6.5f;

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
    [Header("Player isGrounded")]

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;
    #endregion

    #region Variables: Cinemachine

    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;

    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;

    [Tooltip("How far in degrees can you move the camera down")]
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
    [HideInInspector] public Transform cameraTransform;
    #endregion

    #region SM
    public StateMachine movementSM;
    public StandingState standingState;
    public JumpState jumpingState;
    public ShootState shootState;
    public LandingState landingState;
    public ReloadState reloadState;
    public RollingState rollingState;




    // public JumpState jumpState;


    #endregion

    [Header("Rotate Player Towards Camera")]
    public float rotationSmoothTime;
    float turnSmoothVelocity;
    private const float _threshold = 0.01f;


    [Header("Player Shoot")]
    public RaycastWeapon weapon;


    private bool IsCurrentDeviceMouse
    {
        get
        {
            return _playerInput.currentControlScheme == "KeyboardMouse";
        }
    }


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


        //SM
        movementSM = new StateMachine();
        standingState = new StandingState(this, movementSM);
        jumpingState = new JumpState(this, movementSM);
        shootState = new ShootState(this, movementSM);
        landingState = new LandingState(this, movementSM);
        reloadState = new ReloadState(this, movementSM);
        rollingState = new RollingState(this, movementSM);


        gravity *= gravityMultiplier;
        movementSM.Initialize(standingState);


    }
    private void Update()
    {
        if(!HealthSystemPlayer.isAlive) return;
        movementSM.currentState.HandleInput();
        movementSM.currentState.UpdateLogic();
        RotateTowardsCamera();

    }
    private void FixedUpdate()
    {
        movementSM.currentState.UpdatePhysics();

    }
    private void LateUpdate()
    {
        if (!HealthSystemPlayer.isAlive) return;

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
        if(GUIManager.isPauseGame){ return; }

        // if there is an input and camera position is not fixed
        if (input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += input.look.x * deltaTimeMultiplier * sensitivity;
            _cinemachineTargetPitch += input.look.y * deltaTimeMultiplier * sensitivity;
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
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, rotationSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    /*public void FireBullet()
    {
        //weapon.StartShooting();
    }*/

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

/*    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10f, 10f, 400f, 100f));
        string content = movementSM.currentState != null ? movementSM.currentState.ToString() : "(no current state)";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
        GUILayout.EndArea();
    }*/

}

