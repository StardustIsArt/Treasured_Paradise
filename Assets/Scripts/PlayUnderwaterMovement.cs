using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerUnderwaterMovement : MonoBehaviour
{
    [Header("Swim Speed")] public float swimSpeed = 12f;
    public float sprintMaxSpeed = 24f;
    
    [Header("Physics feel")]
    public float underwaterDrag = 4f;
    public float underwaterAngularDrag = 2f;
    public float dragResistanceScale = 0.05f; //for extra drag per metre depth
        
    [Header("References")]
    public Transform cameraTransform;
    public OxygenSystem oxygenSystem;
    
    private Rigidbody _rigidbody;
    [FormerlySerializedAs("_currentDepth")] [SerializeField] private float currentDepth;
    
    Animator _animator;
    private bool _IsSwimming;
    private int _isSwimmingHash;
    private int _swimSpeedHash;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _isSwimmingHash = Animator.StringToHash("IsSwimming"); 
        _swimSpeedHash = Animator.StringToHash("SwimSpeed");
        
        if (_animator == null)
        {
            Debug.LogWarning("No Animator found in children of " + gameObject.name, gameObject);
            enabled = false;
            return;
        }
       
        _rigidbody.useGravity = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        enabled = false;
        
    }

    void Update()
    {
        FaceCameraYaw();
        UpdateDepth();
        _animator.SetFloat(_swimSpeedHash, _rigidbody.linearVelocity.magnitude);
    }

    void FixedUpdate()
    {
        HandleSwim();
        ApplyWaterResistance();
    }

    void FaceCameraYaw()
    {
        transform.rotation = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f);
    }
    
    void UpdateDepth()
    {
        currentDepth = Mathf.Max(0f, -transform.position.y);
        if (oxygenSystem is not null)
        {
            oxygenSystem.SetDepth(currentDepth);
        }
    }

 
    void HandleSwim()
    {   
        float h =  Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        Vector3 swimDirection = cameraTransform.forward * v + cameraTransform.right * h;
        
        bool sprinting = Input.GetKey(KeyCode.LeftShift);
        float speed = sprinting ? sprintMaxSpeed : swimSpeed;
        _IsSwimming = true;
       
     
        
        if (oxygenSystem is not null)
        {
            oxygenSystem.isSprinting = sprinting;
        }

        _rigidbody.AddForce(swimDirection.normalized * speed, ForceMode.Acceleration);
    }  

    void ApplyWaterResistance()
    {
        //Depth increases drag naturally
        float depthDrag = underwaterDrag + (currentDepth * dragResistanceScale);
        _rigidbody.linearDamping = depthDrag;
        _rigidbody.angularDamping = underwaterAngularDrag;
    }

    
    

    public void EnableUnderwaterMode(bool enable)
    {
        _animator.SetBool(_isSwimmingHash, enable);
        _rigidbody.useGravity = false;
        _rigidbody.linearDamping = enable ? underwaterDrag : 0;
        this.enabled = enable;
    }
}