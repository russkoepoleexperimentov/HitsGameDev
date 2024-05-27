using UnityEngine;

namespace Utils
{
    public class RigidbodyState
    {
        private float _mass;
        private float _drag;
        private float _angularDrag;
        private RigidbodyConstraints _constraints;

        public void Save(Rigidbody from)
        {
            _mass = from.mass;
            _drag = from.drag;
            _angularDrag = from.angularDrag;
            _constraints = from.constraints;
        }

        public void Load(Rigidbody to)
        {
            to.mass = _mass;
            to.drag = _drag;
            to.angularDrag = _angularDrag;
            to.constraints = _constraints;
        }
    }
}
