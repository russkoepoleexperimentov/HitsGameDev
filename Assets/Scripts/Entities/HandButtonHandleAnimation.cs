using System.Collections;
using UnityEngine;

public class HandButtonHandleAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 _disengagedPosition;
    [SerializeField] private Vector3 _engagedPosition;
    [SerializeField] private float _pressSpeed = 4;
    [SerializeField] private float _pauseTime = 0.5f;
    [SerializeField] private float _releaseSpeed = 4;

    public void RunAnimation() => StartCoroutine(Animation());

    private IEnumerator Animation()
    {
        for (float i = 0; i <= 1; i += Time.deltaTime * _pressSpeed)
        {
            transform.localPosition = Vector3.Lerp(_disengagedPosition, _engagedPosition, i * i);
            yield return null;
        }
        transform.localPosition = _engagedPosition;

        yield return new WaitForSeconds(_pauseTime);

        for (float i = 0; i <= 1; i += Time.deltaTime * _releaseSpeed)
        {
            transform.localPosition = Vector3.Lerp(_engagedPosition, _disengagedPosition, i);
            yield return null;
        }
        transform.localPosition = _disengagedPosition;
    }

}
