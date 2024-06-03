using UnityEngine;

public class Laser : MonoBehaviour
{
    const string MIRROR_TAG = "Mirror";

    [SerializeField] private LineRenderer _renderer;
    [SerializeField] private int _maxReflections = 4;

    private bool _enabled = false;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            _renderer.gameObject.SetActive(value);
            _enabled = value;
        }
    }

    private Vector3[] _linePoints;

    private void Start()
    {
        if(_renderer == null)
        {
            var obj = new GameObject("PhysGun Pick Line");
            _renderer = obj.AddComponent<LineRenderer>();
            _renderer.startWidth = 0.02f;
            _renderer.endWidth = 0.02f;
            _renderer.useWorldSpace = true;
            _renderer.gameObject.SetActive(false);
        }

        _renderer.positionCount = _maxReflections + 1;

        _linePoints = new Vector3[_maxReflections + 1];
    }

    private void Update()
    {
        if(_enabled)
        {
            _linePoints[0] = transform.position;

            Vector3 origin = transform.position;
            Vector3 direction = transform.forward;
            var points = 1;

            for (int i = 0; i < _maxReflections; i++)
            {
                var ray = new Ray(origin, direction);

                if (!Physics.Raycast(ray, out var hit))
                    break;

                _linePoints[i + 1] = hit.point;
                points++;

                if(hit.transform.TryGetComponent<ILaserHandler>(out var handler))
                {
                    handler.OnLaser(this, ray, hit);
                }

                if (!hit.transform.CompareTag(MIRROR_TAG))
                    break;

                origin = hit.point;
                direction = Vector3.Reflect(direction, hit.normal);
            }

            _renderer.positionCount = points;
            _renderer.SetPositions(_linePoints);
        }
    }
}
