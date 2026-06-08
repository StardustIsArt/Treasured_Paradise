using UnityEngine;

public class PlayerModeSwitch : MonoBehaviour
{
    public PlayerLandMovement landMovement;
    public PlayerUnderwaterMovement underwaterMovement;
    public Rigidbody playerRigidbody;
    public CharacterController characterController;
        
    //Tag water volume trigger as "WaterVolume" (Unity)
    private bool _isUnderwater = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag($"Water Volume") && !_isUnderwater)
        {
            EnterWater();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag($"Water Volume") && _isUnderwater)
        {
            ExitWater();
        }
    }

    void EnterWater()
    {
        _isUnderwater = true;
            
        //Disable land controller
        landMovement.EnableLandMode(false);
        characterController.enabled = false;
            
        //Enable physics + underwater controller
        playerRigidbody.isKinematic = false;
        playerRigidbody.linearVelocity = Vector3.zero;
        underwaterMovement.EnableUnderwaterMode(true);
            
        Debug.Log("Entered Water");
    }

    void ExitWater()
    {
        _isUnderwater = false;
            
        //Disable underwater controller
        underwaterMovement.EnableUnderwaterMode(false);
        playerRigidbody.isKinematic = true;
            
        //Re-enable land controller
        characterController.enabled = true;
        landMovement.EnableLandMode(true);
            
        Debug.Log("Exited Water");
    }
}