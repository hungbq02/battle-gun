using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : Singleton<PlayerController>
{
    [Header("Player")]

    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;

    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    [SerializeField] float jumpHeight;

    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float gravity = -9.81f;
    float gravityMultiplier = 1f;

    public Vector3 _direction;


    [Header("Player isGrounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool isGrounded = true;

    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;

    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;

    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

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

    // player
    public float _verticalVelocity;

    private PlayerInput _playerInput;
    [HideInInspector] public Animator _animator;
    [HideInInspector] public CharacterController _controller;
    public PlayerInputHandler _input;
    public GameObject _mainCamera;

    //ANIMATION STATE
    public const string PLAYER_IDLE = "IdleBattle01_AR_Anim";
    public const string PLAYER_JUMP = "Jump_AR_Anim";
    public const string PLAYER_RUN_FWD = "RunFWD_AR_Anim";
    public const string PLAYER_RUN_BWD = "RunBWD_AR_Anim";
    public const string PLAYER_RUN_LEFT = "RunLeft_AR_Anim";
    public const string PLAYER_RUN_RIGHT = "RunRight_AR_Anim";


    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Vector3 moveDir = Vector3.zero;
    public bool isJumping = false;
    private const float _threshold = 0.01f;


    private bool IsCurrentDeviceMouse
    {
        get
        {
            return _playerInput.currentControlScheme == "KeyboardMouse";
        }
    }


    protected override void Awake()
    {
        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        }
        base.Awake();
    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        _animator = GetComponentInChildren<Animator>();
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInputHandler>();
        _playerInput = GetComponent<PlayerInput>();

    }

    private void Update()
    {
        GroundedCheck();
        ApplyGravity();
        _controller.Move(_direction * MoveSpeed * Time.deltaTime);

    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    public void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        isGrounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target
        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
    }
    //Fake gravity
    private void ApplyGravity()
    {

        if (isGrounded && _verticalVelocity < 0.0f)
        {
            _verticalVelocity = -2.0f;
        }
        else
        {
            _verticalVelocity += gravity * gravityMultiplier * Time.deltaTime;
        }
        //_controller.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
        _direction.y = _verticalVelocity;


    }
    public void Move()
    {
        // normalise input direction
        _direction = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

        if (_direction.sqrMagnitude == 0f) return;
        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        _direction = moveDir;
       // _controller.Move(moveDir.normalized * MoveSpeed * Time.deltaTime);

    }



    public void Jump()
    {
        isJumping = true;
        _input.jump = false;
        _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        _direction.y = _verticalVelocity;

    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (isGrounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }

}

