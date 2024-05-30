using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor
{
    [RequireComponent(typeof(Rigidbody)),
        RequireComponent(typeof(CapsuleCollider))]
    public class ActorMovement : MonoBehaviour
    {
        [SerializeField] private InputAction _movement;
        [SerializeField, Range(0, 1)] private float _accelCoefficient = 0.5f;
        [SerializeField, Range(0, 1)] private float _frictionCoefficient = 0.2f;
        [SerializeField] private float _movementSpeed = 5;

        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;

        private bool _onGround;

        private Vector3 Forward => transform.forward;
        private Vector3 Right => transform.right;

        public Vector3 Velocity => _rigidbody.velocity;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;

            _collider = GetComponent<CapsuleCollider>();
            _collider.material = new PhysicMaterial() { staticFriction = 1, dynamicFriction = 0 };

            _movement.Enable();
        }

        private void FixedUpdate()
        {
            var input = _movement.ReadValue<Vector2>();

            if (_onGround)
            {
                if (input.sqrMagnitude > 0)
                {
                    var velocity = _rigidbody.velocity;
                    var desiredVelocity = (Forward * input.y + Right * input.x) * _movementSpeed;
                    var force = desiredVelocity - velocity;
                    force.y = 0;

                    _rigidbody.AddForce(force * _accelCoefficient, ForceMode.Impulse);
                }
                else
                {
                    var force = -_rigidbody.velocity * _frictionCoefficient;
                    force.y = 0;

                    _rigidbody.AddForce(force, ForceMode.Impulse);
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            _onGround = true;
        }

        private void OnCollisionExit(Collision collision)
        {
            _onGround = false;
        }
    }
}
