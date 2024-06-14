using Actor;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, ILaserHandler
{
    private const float MAX_HEALTH = 100;

    [SerializeField] private ActorMovement _movement;
    [SerializeField] private float _regenerationSpeed = 200f;
    [SerializeField] private float _regenerationCooldown = 1f;

    private float _health = MAX_HEALTH;
    private float _regenTimer;

    public float Health => _health;
    public event Action Die;

    private void Update()
    {
        if (_regenTimer > 0)
            _regenTimer -= Time.deltaTime;
        else
            _health = Mathf.Clamp(_health + Time.deltaTime * _regenerationSpeed, 0, MAX_HEALTH);
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        _regenTimer = _regenerationCooldown;

        if(_health < 0)
        {
            _movement.enabled = false;
            Die?.Invoke();
        }
    }

    public void OnLaser(Laser laser, Ray laserRay, RaycastHit laserHit)
    {
        TakeDamage(laser.Damage * Time.deltaTime);
    }
}
