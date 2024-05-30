using UnityEngine;

public class RigidbodyTrigger : BaseTrigger
{
    [SerializeField] private Rigidbody _rigidbody; 

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == _rigidbody)
            CallOnEnter();
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody == _rigidbody)
            CallOnExit();
    }
}
