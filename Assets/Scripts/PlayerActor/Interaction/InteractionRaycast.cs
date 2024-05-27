using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction
{
    public class InteractionRaycast : MonoBehaviour
    {
        [SerializeField] private float _rayDistance = 1.5f;
        [SerializeField] private LayerMask _rayLayermask = Physics.DefaultRaycastLayers;
        [SerializeField] private QueryTriggerInteraction _triggerInteraction = QueryTriggerInteraction.UseGlobal;
        [SerializeField] private Camera _camera;

        private RaycastHit _hit;
        private bool _hasHit;

        public bool HasHit => _hasHit;
        public RaycastHit Hit => _hit;

        public bool GetHit(Collider collider)
        {
            if (!_hasHit)
                return false;

            return _hit.collider == collider;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void FixedUpdate()
        {
            ShootRay();
        }

        private void ShootRay()
        {
            /*if (MouseCursor.Shown)
            {
                _hasHit = false;
                return;
            }*/

            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            _hasHit = Physics.Raycast(ray, out _hit, _rayDistance, _rayLayermask, _triggerInteraction);
        }
    }
}
