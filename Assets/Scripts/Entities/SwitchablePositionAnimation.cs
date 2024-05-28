using System.Collections;
using UnityEngine;

public class SwitchablePositionAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 _disengagedPosition;
    [SerializeField] private Vector3 _engagedPosition;
    [SerializeField] private float _speed = 0.2f;

    public void SetState(bool state)
    {
        StopAllCoroutines();
        StartCoroutine(Move(state));
    }

    private IEnumerator Move(bool state)
    {
        Vector3 original = transform.localPosition;
        Vector3 target = state ? _engagedPosition : _disengagedPosition;
        for (float i = 0; i <= 1; i += Time.deltaTime * _speed)
        {
            transform.localPosition = Vector3.Lerp(original, target, i);
            yield return null;
        }
        transform.localPosition = target;
    }
}
