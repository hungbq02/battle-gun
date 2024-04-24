using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Player")]

    public float MoveSpeed = 2.0f;
    public float jumpHeight;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;


    #region Variables: Gravity & direction
    public float gravity = -9.81f;
    [HideInInspector] public float _verticalVelocity;
    float gravityMultiplier = 2f;
    public Vector3 _direction;
    Vector3 moveDir = Vector3.zero;
    public Vector3 playerVelocity;
    public float airControl = 0.5f;

    public int MoveXAnimationParameterID;
    public int MoveZAnimationParameterID;
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
    // player
    private PlayerInput _playerInput;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public PlayerInputHandler input;
  //  [HideInInspector] public GameObject _mainCamera;
    [HideInInspector] public Transform cameraTransform;


    #region STRING ANIMATION
    public const string PLAYER_IDLE = "IdleBattle01_HG01_Anim";
    public const string PLAYER_JUMP = "Jump_HG01_Anim";
    public const string PLAYER_RUN_FWD = "RunFWD_HG01_Anim";
    public const string PLAYER_RUN_BWD = "RunBWD_HG01_Anim";
    public const string PLAYER_RUN_LEFT = "RunLeft_HG01_Anim";
    public const string PLAYER_RUN_RIGHT = "RunRight_HG01_Anim";
    public const string PLAYER_SHOT = "ShootSingleshot_HG01_Anim";

    #endregion

    #region SM
    public StateMachine movementSM;
    public StandingState standingState;
    public JumpState jumpingState;
    public ShootState shotState;
    public LandingState landingState;


    // public JumpState jumpState;


    #endregion
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public bool isJumping = false;
    private const float _threshold = 0.01f;

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject parentBullet;
    [SerializeField] private Transform barrelTransform;
    [SerializeField] private float bulletHitMissDistance = 25f;
    private bool IsCurrentDeviceMouse
    {
        get
        {
            return _playerInput.currentControlScheme == "KeyboardMouse";
        }
    }


    protected void Awake()
    {
        // get a reference to our main camera
        /*        if (_mainCamera == null)
                {
                    _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
                }*/
        MoveXAnimationParameterID = Animator.StringToHash("MoveX");
        MoveZAnimationParameterID = Animator.StringToHash("MoveZ");

    }

    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
        controller = GetComponent<CharacterController>();
        input = GetComponent<PlayerInputHandler>();
        _playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
        cameraTransform = Camera.main.transform;


        movementSM = new StateMachine();
        standingState = new StandingState(this, movementSM);
        jumpingState = new JumpState(this, movementSM);
        shotState = new ShootState(this, movementSM);
        landingState = new LandingState(this, movementSM);


        gravity *= gravityMultiplier;
        movementSM.Initialize(standingState);


    }
    private void Update()
    { 
        /*  GroundedCheck();
         ApplyGravity();
          RotateTowardsCamera();
          controller.Move(_direction * MoveSpeed * Time.deltaTime);*/

        movementSM.currentState.HandleInput();
        movementSM.currentState.UpdateLogic();
    }
    private void FixedUpdate()
    {
        movementSM.currentState.UpdatePhysics();

    }
    private void LateUpdate()
    {
        CameraRotation();
    }

    public bool isGrounded()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        return Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);
    }

    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;
            float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

            _cinemachineTargetYaw += input.look.x * deltaTimeMultiplier;
            _cinemachineTargetPitch += input.look.y * deltaTimeMultiplier;
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

        if (isGrounded() && _verticalVelocity < 0.0f)
        {
            _verticalVelocity = -1.0f;
        }
        else
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
        //controller.Move(new Vector3(0, _verticalVelocity, 0) * Time.deltaTime);
        _direction.y = _verticalVelocity;


    }
    /*    public void Move()
        {
            // normalise input direction
            _direction = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

            if (_direction.sqrMagnitude == 0f) return;
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
            _direction = moveDir;

        }*/
    public void Move()
    {
        // normalise input direction
        _direction = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        if (_direction.sqrMagnitude == 0f) return;

        float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;


        moveDir = (Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward).normalized;
        _direction = moveDir;
        Debug.Log(_direction);
    }

    private void RotateTowardsCamera()
    {
        // Get the angle between the character's current forward direction and the camera's forward direction
        float targetAngle = Mathf.Atan2(cameraTransform.forward.x, cameraTransform.forward.z) * Mathf.Rad2Deg;

        // Smoothly rotate the character towards the camera's forward direction
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }



    public void Jump()
    {
        _verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        _direction.y = _verticalVelocity;
    }

    public void Shoot()
    {
        RaycastHit hit;
        GameObject bullet = Instantiate(bulletPrefab, barrelTransform.position, Quaternion.identity, parentBullet.transform);
        BulletController bulletController = bullet.GetComponent<BulletController>();

        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity))
        {

            bulletController.target = hit.point;
            bulletController.hit = true;
        }
        else
        {
            bulletController.target = cameraTransform.position + cameraTransform.forward * bulletHitMissDistance;
            bulletController.hit = false;
        }
        input.shoot = false;

    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }


    /*    private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (isGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Gizmos.DrawSphere(
                new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
                GroundedRadius);
        }*/

}

