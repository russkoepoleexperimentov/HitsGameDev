using Utils;
using UnityEngine;
using Actor;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class ObjectGrabber : MonoBehaviour
    {
        [Header("Input")]
        [SerializeField] private float _rotationSensitivity = 3;
        [SerializeField] private float _massLimit = 25f;
        [SerializeField] private InputAction _rotateButton;
        [SerializeField] private InputAction _throwButton;

        [Header("Joint drive properties")]
        [SerializeField] private float _jointDriveForce = 600;
        [SerializeField] private float _jointDriveDamping = 10;
        [SerializeField] private float _throwVelocity = 100;

        [Header("References")]
        [SerializeField] private Interaction _interaction;
        [SerializeField] private InteractionRaycast _raycast;
        [SerializeField] private Camera _camera;
        [SerializeField] private MouseLook _mouseLook;
        [SerializeField] private ActorMovement _movement;

        private bool _rotate = false;

        private RigidbodyState _cache = new RigidbodyState();
        private Rigidbody _grabbed;
        private Joint _joint;

        public Rigidbody Grabbed => _grabbed;

        public bool IsCarrying => _grabbed != null;

        private void Start()
        {
            _rotateButton.Enable();
            _throwButton.Enable();

            var grabber = new GameObject("Grabber");
            grabber.AddComponent<Rigidbody>().isKinematic = true;
            grabber.transform.SetParent(_camera.transform);

            var joint = grabber.AddComponent<ConfigurableJoint>();
            joint.configuredInWorldSpace = true;
            joint.xDrive = CreateDrive(_jointDriveForce, _jointDriveDamping);
            joint.yDrive = CreateDrive(_jointDriveForce, _jointDriveDamping);
            joint.zDrive = CreateDrive(_jointDriveForce, _jointDriveDamping);
            joint.slerpDrive = CreateDrive(_jointDriveForce, _jointDriveDamping);
            joint.rotationDriveMode = RotationDriveMode.Slerp;
            _joint = joint;
        }

        private JointDrive CreateDrive(float force, float damping)
        {
            JointDrive drive = new JointDrive();
            drive.positionSpring = force;
            drive.positionDamper = damping;
            drive.maximumForce = Mathf.Infinity;
            return drive;
        }

        private void Update()
        {
            HandleCarrying();
            HandleRotation();

            if (_grabbed != null)
            {
                if (_movement.OnGround && _movement.GroundInfo.rigidbody == _grabbed)
                    Drop();

                if (_throwButton.WasPressedThisFrame())
                {
                    var body = _grabbed;
                    Drop();
                    body.AddForce(_camera.transform.forward * _throwVelocity, ForceMode.Acceleration);
                }
            }
        }

        private void HandleRotation()
        {
            if (_grabbed == null)
                return;

            if (_rotateButton.WasPressedThisFrame())
            {
                _rotate = true;
                _mouseLook.enabled = false;
            }

            if (_rotateButton.WasReleasedThisFrame())
            {
                _rotate = false;
                _mouseLook.enabled = true;
            }

            if (_rotate)
            {
                var lookInput = _mouseLook.GetLookInput(); 

                Vector3 rotateVector = new Vector3(-lookInput.x, lookInput.y, 0) * _rotationSensitivity;

                _joint.transform.Rotate(_camera.transform.right * rotateVector.y, Space.World);
                _joint.transform.Rotate(_camera.transform.up * rotateVector.x, Space.World);
            }
        }

        private void HandleCarrying()
        {
            if (!_interaction.UseBinding.WasPressedThisFrame())
                return;

            if(_grabbed != null)
            {
                Drop(true);
                return;
            }

            if (_raycast.HasHit == false)
                return;

            if (_raycast.Hit.rigidbody == null)
                return;

            if (_raycast.Hit.rigidbody.mass > _massLimit)
                return;

            _grabbed = _raycast.Hit.rigidbody;
            _cache.Save(_grabbed);

            _grabbed.drag = 10;
            _grabbed.angularDrag = 5;

            var delta = _grabbed.position - _raycast.Hit.point;
            delta = _camera.transform.InverseTransformVector(delta);

            _joint.transform.position = _grabbed.position;
            _joint.anchor = Vector3.zero;
            _joint.connectedBody = _grabbed;
            _joint.transform.position = _raycast.Hit.point + _camera.transform.forward * delta.z;
        }

        public void Drop(bool resetVelocity = true)
        {
            if (IsCarrying == false)
                throw new System.InvalidOperationException();

            _joint.connectedBody = null;
            _cache.Load(_grabbed);

            if (resetVelocity)
            {
                _grabbed.velocity *= 0.2f;
                _grabbed.angularVelocity *= 0.5f;
            }

            if (_rotate)
            {
                _mouseLook.enabled = true;
                _rotate = false;
            }

            _grabbed = null;
        }
    }
}
