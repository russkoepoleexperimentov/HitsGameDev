using Actor;
using EZCameraShake;
using Interaction;
using UnityEngine;

public class ActorCheckPoints : MonoBehaviour
{
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private ObjectGrabber _grabber;
    [SerializeField] private ActorMovement _movement;
    [SerializeField] private AudioClip _placeClip;
    [SerializeField] private AudioClip _removeClip;
    [SerializeField] private AudioClip _loadClip;

    private bool _active = true;
    private bool _inLoad = false;
    private const string RESET_TRIGGER_NAME = "CheckPointReset";

    private void Start()
    {
        _checkPoint.SetParent(null);
        Deactivate();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) Save();
        if (Input.GetKeyDown(KeyCode.Q)) Load(true);
    }
    private void Save()
    {
        if(_movement.OnGround == false)
        {
            AlertPopup.Instance.Show("Чтобы поставить чекпоинт, Вы должны находиться на поверхности!", 1.3f);
            return;
        }

        _checkPoint.position = transform.position;

        _active = true;
        _checkPoint.gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(_placeClip, _checkPoint.position);
        CameraShaker.Instance.ShakeOnce(2f, 3f, 0.05f, 0.5f);
    }

    public bool Load(bool alerts = false)
    {
        if (!_active)
        {
            if(alerts)AlertPopup.Instance.Show("Чекпойнт не поставлен.", 1.3f);
            return false;
        }

        if(_inLoad) return false;

        _inLoad = true;

        ScreenFade.Singleton.InOut(() =>
        {
            transform.position = _checkPoint.position;
            _rigidbody.velocity = Vector3.zero;

            if (_grabber.IsCarrying)
                _grabber.Drop();

            AudioSource.PlayClipAtPoint(_loadClip, _checkPoint.position);
            CameraShaker.Instance.ShakeOnce(10f, 1, 0.1f, 1f);

            _inLoad = false;
        });

        return true;
    }

    public void Deactivate()
    {
        _active = false;
        _checkPoint.gameObject.SetActive(false);
        //AudioSource.PlayClipAtPoint(_removeClip, _checkPoint.position);
    }

    private void OnTriggerStay(Collider other)
    {
        if(_active && other.CompareTag(RESET_TRIGGER_NAME))
        {
            Deactivate();
            AudioSource.PlayClipAtPoint(_removeClip, transform.position, 0.3f);
            CameraShaker.Instance.ShakeOnce(2f, 3f, 0.05f, 0.5f);
        }
    }
}
