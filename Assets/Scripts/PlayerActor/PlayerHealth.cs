using Actor;
using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour, ILaserHandler
{
    private const float MAX_HEALTH = 100;
    private const float HIT_SOUND_DELAY = 0.05f;

    [SerializeField] private ActorMovement _movement;
    [SerializeField] private float _regenerationSpeed = 200f;
    [SerializeField] private float _regenerationCooldown = 1f;
    [SerializeField] private AudioClip[] _hitSounds;

    private AudioSource _hitFXSource;

    private float _health = MAX_HEALTH;
    private float _regenTimer;

    private float _nextHitSoundTime;

    public float Health => _health;
    public event Action Die;

    private void Start()
    {
        _hitFXSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (_regenTimer > 0)
            _regenTimer -= Time.deltaTime;
        else
            _health = Mathf.Clamp(_health + Time.deltaTime * _regenerationSpeed, 0, MAX_HEALTH);
    }

    public void TakeDamage(float damage)
    {
        if (_health < 0) return;

        _health -= damage;
        _regenTimer = _regenerationCooldown;

        DamageIndicator.Instance.Trigger();

        if(_nextHitSoundTime < Time.time)
        {
            _hitFXSource.PlayOneShot(_hitSounds[UnityEngine.Random.Range(0, _hitSounds.Length)]);
            _nextHitSoundTime = Time.time + HIT_SOUND_DELAY;
        }

        if(_health < 0)
        {
            //_movement.enabled = false;
            Die?.Invoke();
        }
    }

    public void OnLaser(Laser laser, Ray laserRay, RaycastHit laserHit)
    {
        TakeDamage(laser.Damage * Time.deltaTime);
    }
}
