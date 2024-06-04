using UnityEngine;

namespace Actor
{
    public class Footsteps : MonoBehaviour
    {
        private const float VELOCITY_THRESHOLD = 0.1f;

        [SerializeField] private ActorMovement _movement;
        [SerializeField] private float _stepLength = 1.8f;
        [SerializeField] private AudioClip[] _clips;
        [SerializeField] private AudioSource _source;

        private float _stepProgress = 0f;

        private void Update()
        {
            var velocity = _movement.Velocity;
            velocity.y = 0;

            if (_movement.OnGround && velocity.sqrMagnitude > VELOCITY_THRESHOLD)
            {
                _stepProgress += velocity.magnitude * Time.deltaTime;

                if(_stepProgress > _stepLength)
                {
                    var clip = Random.Range(0, _clips.Length);
                    _source.clip = _clips[clip];
                    _source.Play();

                    _stepProgress = 0;
                }
            }
        }
    }
}
