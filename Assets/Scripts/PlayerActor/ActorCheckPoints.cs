using Interaction;
using UnityEngine;

public class ActorCheckPoints : MonoBehaviour
{
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ObjectGrabber _grabber;

    private bool _active = true;


    private void Start()
    {
        _checkPoint.SetParent(null);
        Deactivate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F6)) Save();
        if (Input.GetKeyDown(KeyCode.F7)) Load();
    }

    private void Save()
    {
        _checkPoint.position = transform.position;

        _active = true;
        _checkPoint.gameObject.SetActive(true);
    }

    public bool Load()
    {
        if (!_active)
            return false;

        transform.position = _checkPoint.position;
        _rigidbody.velocity = Vector3.zero;
        
        if(_grabber.IsCarrying)
            _grabber.Drop();

        return true;
    }

    public void Deactivate()
    {
        _active = false;
        _checkPoint.gameObject.SetActive(false);
    }
}
