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
        [SerializeField] private InputAction _crouch;
        [SerializeField, Range(0, 1)] private float _accelCoefficient = 0.5f;
        [SerializeField, Range(0, 1)] private float _frictionCoefficient = 0.2f;
        [SerializeField] private float _movementSpeed = 5;
        [SerializeField] private float _noclipSpeed = 6;
        [SerializeField] private float _ladderMovementSpeed = 6;
        [SerializeField] private float _airMovementSpeed = 3.5f;
        [SerializeField] private float _crouchMovementSpeed = 3.5f;
        [SerializeField] private float _jumpForce = 500;
        [SerializeField] private float _crouchHeight = 1.0f;
        [SerializeField] private Transform _view;

        private float _originalHeight;

        [SerializeField] private float _groundCheckRadius = 0.35f;
        [SerializeField] private float _groundCheckHeight = 0.81f;
        [SerializeField] private Vector3 _groundCheckOffset = Vector3.zero;

        private Rigidbody _rigidbody;
        private CapsuleCollider _collider;

        private bool _onGround;
        private bool _onCrouch = false;
        private bool _onLadder = false;
        private bool _noclip = false;

        private RaycastHit _groundInfo;

        private Vector3 Forward => transform.forward;
        private Vector3 Right => transform.right;

        public Vector3 Velocity => _rigidbody.velocity;

        public bool OnGround => _onGround;
        public RaycastHit GroundInfo => _groundInfo;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.freezeRotation = true;

            _collider = GetComponent<CapsuleCollider>();
            _collider.material = new PhysicMaterial() { staticFriction = 1, dynamicFriction = 0 };
            _originalHeight = _collider.height;

            _movement.Enable();

            SessionProvider.Current.SetActor(gameObject);
            _jump.Enable();
            _crouch.Enable();
        }

        private void Update()
        {
            GroundCheck();

            if (_jump.WasPressedThisFrame() && _onGround)
            {
                var force = _jumpForce * Vector3.up;
                _rigidbody.AddForce(force, ForceMode.Impulse);
            }
            if (_crouch.IsPressed())
            {
                _onCrouch = true;
                _collider.height = _crouchHeight;
            }
            else
            {
                _onCrouch = false;
                _collider.height = _originalHeight;
            }

            if(Input.GetKeyDown(KeyCode.V))
            {
                _noclip = !_noclip;

                if(_noclip)
                {
                    _rigidbody.isKinematic = true;
                    _collider.enabled = false;
                    AlertPopup.Instance.Show("Noclip ON", 0.5f);
                }
                else
                {
                    _rigidbody.isKinematic = false;
                    _collider.enabled = true;
                    AlertPopup.Instance.Show("Noclip OFF", 0.1f);
                }
            }
        }

        private void FixedUpdate()
        {
            var input = _movement.ReadValue<Vector2>();

            if(_noclip)
            {
                if (input.sqrMagnitude > 0)
                {
                    var dir = (_view.forward * input.y + transform.right * input.x).normalized;
                    transform.Translate(dir * _noclipSpeed * Time.deltaTime, Space.World);
                }
                return;
            }

            if (_onLadder)
            {
                LadderMovement(input);
            }
            else
            {
                if (_onGround)
                    GroundMovement(input, _onCrouch);
                else
                    AirMovement(input);
            }
        }

        private void LadderMovement(Vector2 input)
        {
            AirMovement(input);
            var dir = Vector3.Project(_view.forward, Vector3.up).normalized;
            var desiredVelocity = _rigidbody.velocity * 0.2f;
            desiredVelocity.y = 0;
            desiredVelocity += dir * Mathf.Clamp01(input.y) * _ladderMovementSpeed;
            var force = desiredVelocity - _rigidbody.velocity;

            _rigidbody.AddForce(force * _accelCoefficient, ForceMode.Impulse);
        }

        private void GroundMovement(Vector2 input, bool crouch)
        {
            var speed = crouch ? _crouchMovementSpeed : _movementSpeed;

            if (input.sqrMagnitude > 0)
            {
                var velocity = _rigidbody.velocity;
                var desiredVelocity = (Forward * input.y + Right * input.x) * speed;
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

        private void AirMovement(Vector2 input)
        {
            if (input.sqrMagnitude > 0)
            {
                var desiredVelocity = (Forward * input.y + Right * input.x) * _airMovementSpeed;

                var velocity = _rigidbody.velocity;
                velocity.y = 0;

                _rigidbody.AddForce(desiredVelocity * Time.deltaTime, ForceMode.VelocityChange);
            }
        }

        private void GroundCheck()
        {
            var ray = GroundCheckRay();
            _onGround = Physics.SphereCast(ray, _groundCheckRadius, out _groundInfo,
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

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ladder")
            {
                _onLadder = true;
                _rigidbody.useGravity = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Ladder")
            {
                _rigidbody.useGravity = true;
                _onLadder = false;
            }
        }

        private Ray GroundCheckRay() =>
            new Ray(transform.position + _groundCheckOffset, Vector3.down);
    }
}
