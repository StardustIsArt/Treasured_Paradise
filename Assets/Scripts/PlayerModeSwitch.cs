using UnityEngine;

public class PlayerModeSwitch : MonoBehaviour
{
    public PlayerLandMovement landMovement;
    public PlayerUnderwaterMovement underwaterMovement;
    public Rigidbody playerRigidbody;
    public CharacterController characterController;
    public CapsuleCollider swimCollider;

    [Header("Water")] public Transform waterSurface;
    public Collider waterVolume;
    public float swimDepth = 1.4f;
    public float wadeDepth = 0.8f;
    
    //Tag water volume trigger as "WaterVolume" (Unity)
    private bool _isUnderwater;

    void Update()
    {
        float depth = waterSurface.position.y - transform.position.y;
        
        if (!_isUnderwater)
        {
            if (depth > swimDepth && IsOverWater()) EnterWater();
        }
        else
        {
            bool onGround = Physics.CheckSphere(transform.position, landMovement.groundDistance, landMovement.groundMask);
            if (onGround && depth < wadeDepth) ExitWater();
        }
    }

    bool IsOverWater()
    {
        Bounds bounds = waterVolume.bounds;
        Vector3 position = transform.position;
        return position.x > bounds.min.x && position.x < bounds.max.x && position.z > bounds.min.z && position.z < bounds.max.z;;
    }
    
    void EnterWater()
    {
        _isUnderwater = true;
        swimCollider.enabled = true;
            
        //Disable land controller
        landMovement.EnableLandMode(false);
        characterController.enabled = false;
            
        //Enable physics + underwater controller
        playerRigidbody.isKinematic = false;
        playerRigidbody.linearVelocity = Vector3.zero;
        underwaterMovement.EnableUnderwaterMode(true);
            
        Debug.Log("EnteredWater");
    }

    void ExitWater()
    {
        _isUnderwater = false;
       
        //Disable underwater controller
        underwaterMovement.EnableUnderwaterMode(false);
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.isKinematic = true;
            
        //Re-enable land controller
        swimCollider.enabled = false;
        characterController.enabled = true;
        landMovement.EnableLandMode(true);
            
        Debug.Log("ExitedWater");
    }
}