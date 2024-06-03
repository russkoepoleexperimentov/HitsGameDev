using UnityEngine;
using System.Collections.Generic;

public class ObjectButton : BaseActivator
{
    [SerializeField] private float _massThreshold = 4f;
    [SerializeField] private AudioClip _pressSound;
    [SerializeField] private AudioClip _releaseSound;

    private HashSet<Rigidbody> _bodies;

    private void Start()
    {
        _bodies = new HashSet<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody.mass < _massThreshold)
            return;

        if (_bodies.Count == 0)
        {
            _activate?.Invoke();
            AudioSource.PlayClipAtPoint(_pressSound, transform.position);
        }

        _bodies.Add(other.attachedRigidbody);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody.mass < _massThreshold)
            return;

        _bodies.Remove(other.attachedRigidbody);

        if(_bodies.Count == 0)
        {
            _deactivate?.Invoke();
            AudioSource.PlayClipAtPoint(_releaseSound, transform.position);
        }
    }
}
