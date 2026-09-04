using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator _animator;
    int _isWalkingHash;
    private int _isRunningHash;
    private int _velocityHash;
    private float _velocity = 0.01f;
    public float acceleration = 0.1f;
    public float deceleration = 0.5f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        _isWalkingHash = Animator.StringToHash("isWalking"); 
        _isRunningHash = Animator.StringToHash("isRunning");
        _velocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {
        bool isRunning = _animator.GetBool(_isRunningHash);
        bool isWalking = _animator.GetBool(_isWalkingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        
        if (!isWalking && forwardPressed)
        {
            _animator.SetBool(_isWalkingHash, true);
        }
        if (isWalking && forwardPressed)
        {
            _animator.SetBool(_isWalkingHash, false);
        }
        if (!isRunning && forwardPressed && runPressed)
        {
            _animator.SetBool(_isRunningHash, true);
        }
        if (isRunning && (!forwardPressed || !runPressed))
        {
            _animator.SetBool(_isRunningHash, false);
        }
        if (forwardPressed && _velocity < 5.0f)
        {
            _velocity += Time.deltaTime * acceleration;
        }
        if (!forwardPressed && _velocity > 0.0f)
        {
            _velocity -= Time.deltaTime * deceleration;
        }
        if (!forwardPressed && _velocity < 0.0f)
        {
            _velocity =  0.0f;
        }
        
        _animator.SetFloat(_velocityHash, _velocity);
    }
}
