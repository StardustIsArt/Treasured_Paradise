using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    
    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private bool _isGrounded;
    private PlayerInput _playerInput;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = new PlayerInput();
    }

    void Update()
    {
        CheckIfGrounded();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void OnJump()
    {
        if (_isGrounded)
        {
            _rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode2D.Impulse);
        }
      
    }

    void CheckIfGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundDistance, groundMask);
    }

    void OnMovement(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void MovePlayer()
    {
        Vector3 direction = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        direction.Normalize();
        _rb.linearVelocity = new Vector3(direction.x * moveSpeed, _rb.linearVelocity.y, direction.z * moveSpeed);
    }
}
