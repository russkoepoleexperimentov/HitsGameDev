using Interaction;
using System.Collections;
using UnityEngine;

public class HandButton : BaseActivator, IInteractable
{
    [SerializeField] private bool _playTimerAndDeactivateSounds = false;
    [SerializeField] private AudioClip _pressSound;
    [SerializeField] private AudioClip _deactivateSound;
    [SerializeField] private AudioSource _timerSource;
    [SerializeField] private float _cooldown = 0.5f;

    private float _timer = 0f;

    private void Start()
    {
        _timerSource.Stop();
    }

    private void Update()
    {
        if(_timer > 0f)
            _timer -= Time.deltaTime;
    }

    public void OnInteraction()
    {
        if (_timer > 0f) return;

        StartCoroutine(Press());
    }

    private IEnumerator Press()
    {
        _activate?.Invoke();
        AudioSource.PlayClipAtPoint(_pressSound, transform.position);

        _timer = _cooldown;

        if (_playTimerAndDeactivateSounds)
            _timerSource.Play();

        yield return new WaitWhile(() => _timer > 0);

        _deactivate?.Invoke();

        if (_playTimerAndDeactivateSounds)
        {
            _timerSource.Stop();
            AudioSource.PlayClipAtPoint(_deactivateSound, transform.position);
        }
    }
}
