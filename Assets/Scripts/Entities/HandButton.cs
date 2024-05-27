using Interaction;
using UnityEngine;
using UnityEngine.Events;

public class HandButton : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent _onPress;
    [SerializeField] private AudioClip _pressSound;
    [SerializeField] private float _cooldown = 0.5f;

    private float _timer = 0f;

    public UnityEvent OnPress => _onPress;

    private void Update()
    {
        if(_timer > 0f)
            _timer -= Time.deltaTime;
    }

    public void OnInteraction()
    {
        if (_timer > 0f) return;

        _onPress?.Invoke();
        AudioSource.PlayClipAtPoint(_pressSound, transform.position);

        _timer = _cooldown;
    }
}
