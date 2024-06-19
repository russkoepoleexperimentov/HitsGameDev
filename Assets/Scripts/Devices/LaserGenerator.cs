using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGenerator : MonoBehaviour
{
    [SerializeField] private Laser _laser;
    [SerializeField] private TimeBasedRotator _rotator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private bool _enabled = true;

    public bool Enabled
    {
        get => _enabled;

        set
        {
            _enabled = value;
            _laser.Enabled = value;
            _rotator.IsEnabled = value;
            if(value)
                _audioSource?.Play();
            else
                _audioSource?.Stop();
        }
    }

    public void Disable() => Enabled = false;
    public void Enable() => Enabled = true;

    private void Start()
    {
        if(_audioSource)
            _audioSource.loop = true;

        Enabled = _enabled;
    }
}
