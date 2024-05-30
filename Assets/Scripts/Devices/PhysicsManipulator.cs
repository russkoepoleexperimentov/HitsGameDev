using Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsManipulator : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _speed = 0.7f;
    [SerializeField] private Vector3 _angularVelocity = Vector3.up * 1;

    private bool _mode = false;

    private void Update()
    {

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(_camera.ScreenPointToRay(Input.mousePosition), out var hit) && 
                hit.rigidbody)
            {
                var rigidbody = hit.rigidbody;
                var dir = (hit.point - _camera.transform.position).normalized;

                float factor = (_mode ? 1 : -1);
                rigidbody.velocity = dir * _speed * factor;
                rigidbody.AddTorque(_angularVelocity * factor);
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            _mode = !_mode;
        }
    }
}
