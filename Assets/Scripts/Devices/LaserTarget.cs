using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserTarget : MonoBehaviour, ILaserHandler
{
    private const float DEACTIVATION_TIME = 0.2f;

    [SerializeField] private UnityEvent _activated;
    [SerializeField] private UnityEvent _deactivated;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private TimeBasedRotator _rotator;

    private float _timer = 0;

    private void Start()
    {
        _audioSource.loop = true;
        _audioSource.Play();
    }

    public void OnLaser(Laser laser, Ray laserRay, RaycastHit laserHit)
    {
        if (_timer <= 0)
        {
            _activated?.Invoke();
            _rotator.IsEnabled = true;
        }

        _timer = DEACTIVATION_TIME;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                _deactivated?.Invoke();
                _rotator.IsEnabled = false;
            }
        }

        _audioSource.pitch = _rotator.SpeedFactor;
    }
}
