using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Actor
{
    public class Headbob : MonoBehaviour
    {
        private const float VELOCITY_THRESHOLD = 0.1f;

        [SerializeField] private ActorMovement _movement;
        [SerializeField] private float _velocityMultiplier = 0.2f;
        [SerializeField] private Vector2 _amount = new Vector2(0.1f, 0.05f);

        private float _stepProgress = 0;

        private void Update()
        {
            var velocity = _movement.Velocity;
            velocity.y = 0;

            if(velocity.sqrMagnitude > VELOCITY_THRESHOLD)
            {
                _stepProgress += velocity.magnitude * _velocityMultiplier * Time.deltaTime;

                transform.localPosition = Vector3.right * _amount.x * Mathf.Sin(_stepProgress) +
                    Vector3.up * _amount.y * Mathf.Abs(Mathf.Cos(_stepProgress));
            }
        }
    }
}
