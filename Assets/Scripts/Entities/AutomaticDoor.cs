using UnityEngine;

public class AutomaticDoor : MonoBehaviour
{
    [SerializeField] private SwitchablePositionAnimation[] _planes;
    [SerializeField] private AudioClip _openClip;
    [SerializeField] private AudioClip _closeClip;

    private bool _isOpen = false;

    public void Open()
    {
        if (_isOpen) return;
        _isOpen = true;

        if (_openClip != null)
            AudioSource.PlayClipAtPoint(_openClip, transform.position);

        foreach (var plane in _planes)
        {
            plane.SetState(true);
        }
    }

    public void Close()
    {
        if (!_isOpen) return;
        _isOpen = false;

        if (_closeClip != null)
            AudioSource.PlayClipAtPoint(_closeClip, transform.position);

        foreach (var plane in _planes)
        {
            plane.SetState(false);
        }
    }
}
