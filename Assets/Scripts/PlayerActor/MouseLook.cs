using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private InputAction _lookInput;
        [SerializeField] private float _sensitivity = 30f;

        [SerializeField] private Transform _body;
        [SerializeField] private Transform _head;

        private float _xRotation = 0f;

        public Vector2 GetLookInput() => _lookInput.ReadValue<Vector2>() * Time.deltaTime;

        private void Start()
        {
            _lookInput.Enable();

            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            var input = GetLookInput() * _sensitivity;

            _xRotation = Mathf.Clamp(_xRotation - input.y, -90, 90);

            _head.localEulerAngles = Vector3.right * _xRotation;

            _body.Rotate(Vector3.up * input.x);
        }
    }
}
