using UnityEngine;
using UnityEngine.Serialization;

public class PlayerLandMovement : MonoBehaviour
{
    //private static readonly int speed1 = Animator.StringToHash("Speed");

    
    [Header("Movement")] 
    public float walkSpeed = 2.5f;
    public float runSpeed = 5f;
    public float acceleration = 0.1f;
    public float deceleration = 0.5f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
   
    [Header("Ground Check")] 
    public float groundDistance = 1f;
    public LayerMask groundMask;
    
    [SerializeField] private Transform playerCamera;
    
    Animator _animator;
    private CharacterController _characterController;
    [SerializeField] private Animator characterAnimator;
    private Vector3 _velocity;
    private float _currentVelocityX;
    private float _currentVelocityZ;
    private bool _isGrounded;
    private bool _isRunning;

    private int _velocityXHash;
    private int _velocityZHash;
    private int _isWalkingHash;
    private int _isRunningHash;
    //  private float _velocity = 0.01f;
    
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        
        if (_animator == null)
        {
            Debug.LogError("characterAnimator is not assigned on " + gameObject.name, gameObject);
            enabled = false;
            return;
        }
        _isWalkingHash = Animator.StringToHash("isWalking"); 
        _isRunningHash = Animator.StringToHash("isRunning");
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
        FaceCameraDirection();
        HandleMove();
        HandleAnimation();
    }

    void FaceCameraDirection()
    {
        Vector3 camForward = playerCamera.forward;
        camForward.y = 0f;
        if (camForward.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(camForward.normalized);
        }
    }

    void HandleMove()
    {
        if (_isGrounded && _velocity.y < 0f) _velocity.y = -2f;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        _isRunning = Input.GetKey(KeyCode.LeftShift) && v > 0f;
        float targetSpeed = _isRunning ? runSpeed : walkSpeed;
        float targetVelocityX = h * targetSpeed;
        float targetVelocityZ = v * targetSpeed;

        _currentVelocityX = Mathf.MoveTowards(_currentVelocityX, targetVelocityX,
            (Mathf.Abs(targetVelocityX) > 0.01f ? acceleration : deceleration) * Time.deltaTime);
        _currentVelocityZ = Mathf.MoveTowards(_currentVelocityZ, targetVelocityZ,
            (Mathf.Abs(targetVelocityZ) > 0.01f ? acceleration : deceleration) * Time.deltaTime);

        Vector3 move = transform.right * _currentVelocityX + transform.forward * _currentVelocityZ;
        _characterController.Move(move * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * Time.deltaTime);
    }

    void HandleAnimation()
    {
        _animator.SetFloat(_velocityXHash, _currentVelocityX);
        _animator.SetFloat(_velocityZHash, _currentVelocityZ);

        bool isMoving = Mathf.Abs(_currentVelocityX) > 0.01f || Mathf.Abs(_currentVelocityZ) > 0.01f;
        _animator.SetBool(_isWalkingHash, isMoving && !_isRunning);
        _animator.SetBool(_isRunningHash, isMoving && _isRunning);
    }

    void CheckIfGrounded()
    {
        Vector3 feetPosition = transform.position + Vector3.down * (_characterController.height / 2f);
        _isGrounded = Physics.CheckSphere(feetPosition, groundDistance, groundMask);
    }
   
   

    //Called by PlayerSwitchMode when entering water
    public void EnableLandMode(bool enable)
    {
        _characterController.enabled = enable;
        this.enabled = enable;
    }
    
}