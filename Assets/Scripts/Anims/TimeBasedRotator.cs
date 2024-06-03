using UnityEngine;

public class TimeBasedRotator : MonoBehaviour
{
    [SerializeField] private Vector3 _rotateVector = Vector3.forward;
    [SerializeField] private Space _space = Space.Self;
    [SerializeField] private float _accelerateTime = 0.7f;
    [SerializeField] private float _decelerateTime = 3f;
    [SerializeField] private bool _initialState = false;

    private float _speed = 0;

    public bool IsEnabled { get; set; } = true;

    public float SpeedFactor => _speed;

    private void Start()
    {
        IsEnabled = _initialState;
    }

    private void Update()
    {
        if(IsEnabled)
            _speed = Mathf.Clamp01(_speed + Time.deltaTime / _accelerateTime);
        else
            _speed = Mathf.Clamp01(_speed - Time.deltaTime / _decelerateTime);

        transform.Rotate(_rotateVector * _speed * Time.deltaTime, _space);
    }
}
