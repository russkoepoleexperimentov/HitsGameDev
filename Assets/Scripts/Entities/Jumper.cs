using General;
using UnityEngine;
using UnityEngine.Events;

public class Jumper : MonoBehaviour
{
    [SerializeField] private Vector3 _direction = Vector3.forward + Vector3.up;
    [SerializeField] private float _velocity;
    [SerializeField] private AudioClip _clip;
    [SerializeField] private UnityEvent _jumpPerformed;

    [Header("Gizmos")]
    [SerializeField] private float _debugFlyTime = 3f;
    [SerializeField] private float _debugFlyTimeStep = 0.02f;

    private void OnDrawGizmosSelected()
    {
        var velo = _direction.normalized * _velocity;
        var pos = transform.position;

        for(float t = 0; t < _debugFlyTime; t += _debugFlyTimeStep)
        {
            var lastPos = pos;
            velo += Physics.gravity * _debugFlyTimeStep;
            pos += velo * _debugFlyTimeStep;
            Debug.DrawLine(lastPos, pos, Color.green);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        var body = other.attachedRigidbody;

        if (body)
        {

            var grabber = SessionProvider.Current.Grabber;

            if (body == grabber.Grabbed)
                grabber.Drop();

            var velo = _direction.normalized * _velocity;
            body.velocity = velo;
            body.angularVelocity = Vector3.zero;

            _jumpPerformed?.Invoke();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
        }
    }
}
