using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : MonoBehaviour
{
   [SerializeField] private Transform playerCameraTransform;
   [SerializeField] private Transform objectGrabPointTransform;
   [SerializeField] private LayerMask pickUpLayerMask;
   
   
   private float pickUpDistance = 2f; 
   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.E))
      {
         if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
         {
            if (raycastHit.transform.TryGetComponent(out ObjectGrabbable objectGrabbable))
            {
               objectGrabbable.Grab(objectGrabPointTransform);
            }
         }
      }
   }
}
