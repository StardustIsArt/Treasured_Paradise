using UnityEngine;

public class PlayerLandMovement : MonoBehaviour
{
    [Header("Movement")] 
    public float walkSpeed = 5f;
    public float runSpeed = 10f;
    public float acceleration = 15f;
    public float deceleration = 25f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
   
    [Header("Ground Check")] 
    public float groundDistance = 1f;
    public LayerMask groundMask;
    
    [SerializeField] private Transform playerCamera;
    
    Animator _animator;
    private CharacterController _characterController;
    private Vector3 _velocity;
    private float _currentVelocityX;
    private float _currentVelocityZ;
    private bool _isGrounded;
    private bool _isRunning;
    private bool _hasJumped;

    private int _velocityXHash;
    private int _velocityZHash;
    private int _isWalkingHash;
    private int _isRunningHash;
    private int _isJumpingHash;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _isJumpingHash = Animator.StringToHash("IsJumping");
        
        if (_animator == null)
        {
            Debug.LogError("No Animator found in children of " + gameObject.name, gameObject);
            enabled = false;
            return;
        }
        _isWalkingHash = Animator.StringToHash("IsWalking"); 
        _isRunningHash = Animator.StringToHash("IsRunning");
        _velocityXHash = Animator.StringToHash("VelocityX");
        _velocityZHash = Animator.StringToHash("VelocityZ");
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckIfGrounded();
        HandleMove();
        FaceCameraYaw();
        HandleAnimation();
    }

    void CheckIfGrounded()
    {
        _isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);
    }

    void HandleMove()
    {
        if (_isGrounded && _velocity.y < 0f) _velocity.y = -2f;
        
        float strafe = Input.GetAxis("Horizontal");  // A/D
        float forward = Input.GetAxis("Vertical");  // W/S
        _isRunning = Input.GetKey(KeyCode.LeftShift) && forward > 0f;
        
        float targetSpeed = _isRunning ? runSpeed : walkSpeed;
        float targetVelocityZ = forward * targetSpeed;
        float targetVelocityX = strafe * targetSpeed;

        _currentVelocityX = Mathf.MoveTowards(_currentVelocityX, targetVelocityX,
            (Mathf.Abs(targetVelocityX) > 0.01f ? acceleration : deceleration) * Time.deltaTime);
        _currentVelocityZ = Mathf.MoveTowards(_currentVelocityZ, targetVelocityZ,
            (Mathf.Abs(targetVelocityZ) > 0.01f ? acceleration : deceleration) * Time.deltaTime);

        Vector3 move = transform.right * _currentVelocityX + transform.forward * _currentVelocityZ;
        _characterController.Move(move * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            _hasJumped = true;
        }

        if (_hasJumped && _isGrounded && _velocity.y < 0f)
        {
            _hasJumped = false;
        }

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    void FaceCameraYaw()
    {
        // Body matches the camera's horizontal facing; pitch stays on the camera only
        transform.rotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);
    }

    void HandleAnimation()
    {
        _animator.SetFloat(_velocityXHash, _currentVelocityX);
        _animator.SetFloat(_velocityZHash, _currentVelocityZ);

        bool isMoving = Mathf.Abs(_currentVelocityX) > 0.01f || Mathf.Abs(_currentVelocityZ) > 0.01f;
        _animator.SetBool(_isWalkingHash, isMoving && !_isRunning);
        _animator.SetBool(_isRunningHash, isMoving && _isRunning);
        _animator.SetBool(_isJumpingHash, _hasJumped);
    }


   

    //Called by PlayerSwitchMode when entering water
    public void EnableLandMode(bool enable)
    {
        _characterController.enabled = enable;
        this.enabled = enable;
    }
    
}