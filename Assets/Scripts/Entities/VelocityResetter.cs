using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityResetter : MonoBehaviour
{

    public void OnCollisionEnter(Collision other)
    {
        if (other.collider.attachedRigidbody)
        {
            other.collider.attachedRigidbody.velocity = Vector3.zero;
        }
    }
}
