using UnityEngine;

public class MoveTargetPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 _target;
    [SerializeField] private float _speed = .7f;

    private Vector3 _basePosition;

    public bool IsMoving { get; set; } = false;

    private void Start()
    {
        _basePosition = transform.localPosition;
    }

    private void Update()
    {
        if(IsMoving)
            Proceed();
    }

    private void Proceed()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, _target, 
            _speed * Time.deltaTime);
    }

    public void ResetPosition() => transform.localPosition = _basePosition;
}
