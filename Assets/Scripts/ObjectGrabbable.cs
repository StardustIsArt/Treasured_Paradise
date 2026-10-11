using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
  private Rigidbody objectRigidbody;
  private Transform objectGrabPointTransform;

  private void Awake()
  {
    objectRigidbody = GetComponent<Rigidbody>();
  }
  public void Grab(Transform objectGrabPointTransform)
  {
    this.objectGrabPointTransform = objectGrabPointTransform;
  }

  public void FixedUpdate()
  {
    if (!objectGrabPointTransform)
    {
      objectRigidbody.MovePosition(objectGrabPointTransform.position);
    }
  }
}
