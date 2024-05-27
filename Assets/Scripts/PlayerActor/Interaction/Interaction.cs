using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class Interaction : MonoBehaviour
    {
        [SerializeField] private InteractionRaycast _raycast;
        [SerializeField] private ObjectGrabber _grabber;
        [SerializeField] private InputAction _useBinding;

        public InputAction UseBinding => _useBinding;

        private void Start()
        {
            _useBinding.Enable();
        }

        private void Update()
        {
            if (!_useBinding.WasPressedThisFrame())
                return;

            if (_grabber.IsCarrying)
                return;

            if (_raycast.HasHit == false)
                return;

            IInteractable interactable;

            if (!_raycast.Hit.collider.TryGetComponent(out interactable))
                if (_raycast.Hit.rigidbody && !_raycast.Hit.rigidbody.TryGetComponent(out interactable))
                    return;

            if (interactable == null)
                return;

            if (_grabber.IsCarrying) _grabber.Drop();
            interactable.OnInteraction();
        }
    }

    public interface IInteractable
    {
        public void OnInteraction();
    }
}
