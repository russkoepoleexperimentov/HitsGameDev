using General;
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
        [SerializeField] private InputAction _jump;
        [SerializeField, Range(0, 1)] private float _accelCoefficient = 0.5f;
        [SerializeField, Range(0, 1)] private float _frictionCoefficient = 0.2f;
        [SerializeField] private float _movementSpeed = 5;
        [SerializeField] private float _airMovementSpeed = 3;
        [SerializeField] private float _jumpForce = 500;

        [SerializeField] private float _groundCheckRadius = 0.35f;
        [SerializeField] private float _groundCheckHeight = 0.81f;
        [SerializeField] private Vector3 _groundCheckOffset = Vector3.zero;

        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;

        private bool _onGround;

        private Vector3 Forward => transform.forward;
        private Vector3 Right => transform.right;

        public Vector3 Velocity => _rigidbody.velocity;

        public bool OnGround => _onGround;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;

            _collider = GetComponent<CapsuleCollider>();
            _collider.material = new PhysicMaterial() { staticFriction = 1, dynamicFriction = 0 };

            _movement.Enable();

            SessionProvider.Current.SetActor(gameObject);
            _jump.Enable();
        }

        private void Update()
        {
            GroundCheck();

            if (_jump.WasPressedThisFrame() && _onGround)
            {
                var force = _jumpForce * Vector3.up;
                _rigidbody.AddForce(force, ForceMode.Impulse);
            }
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
            else
            {
                if (input.sqrMagnitude > 0) 
                {
                    var desiredVelocity = (Forward * input.y + Right * input.x) * _airMovementSpeed;

                    _rigidbody.AddForce(desiredVelocity * Time.deltaTime, ForceMode.Impulse);
                }
                else
                {
                    var force = -_rigidbody.velocity * _frictionCoefficient;
                    force.y = 0;

                    //_rigidbody.AddForce(force, ForceMode.Impulse);
                }
            }
        }

        private void GroundCheck()
        {
            var ray = GroundCheckRay();
            _onGround = Physics.SphereCast(ray, _groundCheckRadius, out var hit,
                (_groundCheckHeight - _groundCheckRadius), Physics.DefaultRaycastLayers, 
                QueryTriggerInteraction.Ignore);
        }

        private void OnDrawGizmosSelected()
        {
            var ray = GroundCheckRay();
            var start = ray.origin;
            var end = ray.origin + ray.direction.normalized * (_groundCheckHeight - _groundCheckRadius);
            Debug.DrawLine(start, end);
            Gizmos.DrawWireSphere(end, _groundCheckRadius);
        }

        private Ray GroundCheckRay() =>
            new Ray(transform.position + _groundCheckOffset, Vector3.down);
    }
}
