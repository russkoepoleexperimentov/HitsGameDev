using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(NPCRoller))]
public class RollerBrain : MonoBehaviour, IGrabRayContext
{
    [SerializeField] private float _stoppingDistance = 1f;
    [SerializeField] private Physgun _physgun;
    [SerializeField] private Transform _head;

    private NPCRoller _roller;

    private void Start()
    {
        _roller = GetComponent<NPCRoller>();
        _roller.CurrentState = NPCRoller.State.Stop;

        StartCoroutine(TurnHead());
    }

    public void Grab(Transform transform) 
    {
        _head.LookAt(transform.position);
        _physgun.Grab(this);
    }//=> StartCoroutine(GrabRoutine(transform));

    public void MoveObjectTo(Transform what, Transform place) => StartCoroutine(MoveObjectToRoutine(what, place.position));
    public void MoveObjectTo(Transform place) => MoveObjectTo(_physgun.Grabbed.transform, place);

    public void GoAndStand(Vector3 target) => StartCoroutine(GoAndRoutine(target, _stoppingDistance, StandRoutine()));
    public void GoAndStand(Transform target) => GoAndStand(target.position);

    private IEnumerator GoAndRoutine(Vector3 target, float distanceThreshold, IEnumerator next)
    {
        _roller.Jump(0.2f);
        _roller.CurrentState = NPCRoller.State.FollowTarget;
        _roller.Target = target;

        var distanceThresholdSquared = distanceThreshold * distanceThreshold;

        while(true)
        {
            var distanceSquared = (target - transform.position).sqrMagnitude;

            if (distanceSquared < distanceThresholdSquared)
                break;

            yield return null;
        }

        if(next != null)
            yield return StartCoroutine(next);
        else
            _roller.CurrentState = NPCRoller.State.Stop;
    }

    private IEnumerator StandRoutine()
    {
        _roller.CurrentState = NPCRoller.State.Stop;
        yield return new WaitWhile(() => _roller.Rigidbody.velocity.sqrMagnitude > 0.25f);
        _roller.CurrentState = NPCRoller.State.Stand;
        while (!_roller.OnFoot && _roller.OnGround)
        {
            _roller.Jump(0.5f);
            yield return new WaitForSeconds(0.5f);
            yield return new WaitWhile(() => !_roller.OnGround);
        }
        _roller.CurrentState = NPCRoller.State.Stop;
    }

    private IEnumerator TurnHead()
    {
        while (true)
        {
            Vector3 target;

            if (_physgun.Grabbed == null)
            {
                var vel = _roller.Rigidbody?.velocity ?? Vector3.zero;
                if (vel.sqrMagnitude > 0.2f)
                    target = vel;
                else
                    target = transform.forward;
            }
            else
            {
                target = _physgun.Grabbed.position - transform.position;
            }

            Quaternion look = Quaternion.LookRotation(target, Vector3.up);
            _head.rotation = Quaternion.Lerp(_head.rotation, look, Time.deltaTime * 5);
            yield return null;
        }
    }

    private IEnumerator GrabRoutine(Transform what)
    {
        _head.LookAt(what.position);
        _physgun.Grab(this);
        yield return null;
    }

    private IEnumerator HeldObjRoutine()
    {
        while (true)
        {
            var pos = _physgun.Grabbed.position;
            var targetPos = pos;
            targetPos.y = (_head.position + Vector3.up * 2).y;
            var dir = targetPos - pos;
            _physgun.VelocityOffset = dir;

            yield return null;
        }
    }

    private IEnumerator MoveObjectToRoutine(Transform what, Vector3 place, float sqrThreshold = 0.5f)
    {
        if(_physgun.Grabbed == null)
            Grab(what);

        var dir = place - transform.position;
        var dist = dir.magnitude - _physgun.GrabDistance - sqrThreshold;
        var dest = transform.position + dir.normalized * dist;

        var routine = StartCoroutine(HeldObjRoutine());
        yield return StartCoroutine(GoAndRoutine(dest, _stoppingDistance, null));

        while ((place - _physgun.Grabbed.position).sqrMagnitude > sqrThreshold)
        {
            _physgun.VelocityOffset = (place - _physgun.Grabbed.position);
            yield return null;
        }

        StopCoroutine(routine);

        _physgun.Release(false);
    }

    public Ray GetRay()
    {
        return new Ray(_physgun.transform.position, _physgun.transform.forward);
    }
}
