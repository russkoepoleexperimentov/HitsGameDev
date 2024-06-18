using Actor;
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

    // мчд - малая черная дыра если что)))))
    private void Save()
    {
        if(_movement.OnGround == false)
        {
            AlertPopup.Instance.Show("Чтобы поставить МЧД, Вы должны находиться на поверхности!", 1.3f);
            return;
        }

        _checkPoint.position = transform.position;

        _active = true;
        _checkPoint.gameObject.SetActive(true);
        AudioSource.PlayClipAtPoint(_placeClip, _checkPoint.position);
    }

    public bool Load()
    {
        if (!_active)
        {
            AlertPopup.Instance.Show("МЧД не выставлена.", 1.3f);
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
}
