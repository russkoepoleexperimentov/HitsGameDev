using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NPCRoller : MonoBehaviour
{
    public enum State : byte
    {
        None = 0,
        FollowTarget,
        Stop,
        Stand
    }

    [Header("General")]
    [SerializeField] private float _rollTorque = 2f;
    [SerializeField] private float _jumpVelocity = 5f;
    [SerializeField] private GameObject _legs;

    [Header("Stand")]
    [SerializeField] private float _standAdjustFactor = 0.2f;
    [SerializeField] private float _maxDeflectionAngle = 30f;
     
    [Header("FX")]
    [SerializeField] private AudioSource _source;
    [SerializeField] private float _rollClipReferenceVelocity = 10f;

    private Rigidbody _rigidbody;
    private bool _onGround;

    public State CurrentState { get; set; } = State.None;
    public Vector3 Target { get; set; }
    public Rigidbody Rigidbody => _rigidbody;
    public bool OnGround => _onGround;
    public bool OnFoot => Vector3.Angle(transform.up, Vector3.up) < _maxDeflectionAngle;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        PlayRollSound();

        UpdateState();
    }

    private void UpdateState()
    {
        switch(CurrentState)
        {
            case State.FollowTarget:
                FollowingTarget();
                break;

            case State.Stand:
                Standing();
                break;

            case State.Stop:
                Stopping(); 
                break;

            case State.None:
                break;
        }
    }

    private void FollowingTarget()
    {
        var direction = (Target - transform.position).normalized;
        var torqueAxis = new Vector3(direction.z, 0, -direction.x);
        _rigidbody.AddTorque(torqueAxis * _rollTorque);
    }

    private void Stopping()
    {
        _rigidbody.angularVelocity = Vector3.zero;
    }

    private void Standing()
    {
        var deltaRotation = Quaternion.FromToRotation(transform.up, Vector3.up);

        float deltaAngle;
        Vector3 deltaAxis;
        deltaRotation.ToAngleAxis(out deltaAngle, out deltaAxis);

        if (Mathf.Abs(deltaAngle) < _maxDeflectionAngle)
            return;

        _rigidbody.AddTorque(deltaAxis.normalized * deltaAngle * _standAdjustFactor);
    }

    public void Jump(float multiplier = 1f)
    {
        if (!_onGround)
            return;

        _rigidbody.AddForce(Vector3.up * _jumpVelocity * multiplier, ForceMode.VelocityChange);
    }

    private void OnCollisionStay(Collision collision)
    {
        _onGround = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        _onGround = false;
    }

    private void PlayRollSound()
    {
        var pitch = _rigidbody.velocity.magnitude / _rollClipReferenceVelocity;
        pitch = Mathf.Clamp01(pitch);
        _source.pitch = pitch;
    }
}
