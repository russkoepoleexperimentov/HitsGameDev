using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private Vector3 _direction = Vector3.forward + Vector3.up;
    [SerializeField] private float _velocity;

    [Header("Gizmos")]
    [SerializeField] private float _debugFlyTime = 3f;
    [SerializeField] private float _debugFlyTimeStep = 0.02f;

    private void OnDrawGizmosSelected()
    {
        var velo = _direction.normalized * _velocity;
        var pos = transform.position;

        for(float t = 0; t < _debugFlyTime; t += _debugFlyTimeStep)
        {
            var lastPos = pos;
            velo += Physics.gravity * _debugFlyTimeStep;
            pos += velo * _debugFlyTimeStep;
            Debug.DrawLine(lastPos, pos, Color.green);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.attachedRigidbody)
        {
            var velo = _direction.normalized * _velocity;
            other.attachedRigidbody.velocity = velo;
        }
    }
}
