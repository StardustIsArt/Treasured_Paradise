using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
   [SerializeField] private Transform playerCameraTransform;
   [SerializeField] private Transform objectGrabPointTransform;
   [SerializeField] private LayerMask pickUpLayerMask;
   
   
   private float pickUpDistance = 2f; 
   private ObjectGrabbable objectGrabbable;
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.E))
      {
         if(!objectGrabbable) {
            // Not carrying an object, try to grab
            if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
            {
               if (raycastHit.transform.TryGetComponent(out objectGrabbable))
               {
                  objectGrabbable.Grab(objectGrabPointTransform);
               }
            }
         }
         else
         {
            // Currently holding object, drop
            objectGrabbable.Drop();
            objectGrabbable = null;
         }         
      }
   }
}
