using UnityEngine;

public class PlayerModeSwitch : MonoBehaviour
{
    public PlayerLandMovement landMovement;
    public PlayerUnderwaterMovement underwaterMovement;
    public Rigidbody playerRigidbody;
    public CharacterController characterController;
    public CapsuleCollider swimCollider;
    
    //Tag water volume trigger as "WaterVolume" (Unity)
    private bool _isUnderwater;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterVolume") && !_isUnderwater)
        {
            EnterWater();
        }
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

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("WaterVolume") && !_isUnderwater) return;

        if (other.bounds.Contains(transform.position)) return;
        
        ExitWater();
    }

    void ExitWater()
    {
        _isUnderwater = false;
       
            
        //Disable underwater controller
        underwaterMovement.EnableUnderwaterMode(false);
        playerRigidbody.isKinematic = true;
            
        //Re-enable land controller
        swimCollider.enabled = false;
        characterController.enabled = true;
        landMovement.EnableLandMode(true);
            
        Debug.Log("ExitedWater");
    }
}