using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidbody;
    private Transform objectGrabPointTransform;

    private void Awake(){
        objectRigidbody = GetComponent<Rigidbody>();
    }

    public void Grab (Transform objectGrabPointTransform) {
        // this.transform.position = new Vector3(0, -30, 90); 
        DisableCollider();       
        this.objectGrabPointTransform = objectGrabPointTransform;
        objectRigidbody.useGravity = false;
    }

    public void DisableCollider () {
        this.GetComponent<BoxCollider>().enabled = false;

    }

    private void FixedUpdate(){
        if (objectGrabPointTransform != null)
        {
            objectRigidbody.MovePosition(new Vector3(objectGrabPointTransform.position.x,
                                                objectGrabPointTransform.position.y - 0.5f,
                                                objectGrabPointTransform.position.z ));            
        }
    }
}
