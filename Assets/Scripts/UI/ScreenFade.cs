using System;
using System.Collections;
using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private float _inTime = 0.7f;
    [SerializeField] private float _outTime = 1.5f;

    private bool _state = false;

    public static ScreenFade Singleton { get; private set; }

    private void Awake()
    {
        if (Singleton == null)
            Singleton = this;
        else
            Destroy(this);
    }

    private void Update()
    {
        var alpha = _canvasGroup.alpha;
        alpha = Mathf.Clamp01(alpha + Time.deltaTime / (_state ? _inTime : -_outTime));
        _canvasGroup.alpha = alpha;
    }

    public void In() => _state = true;

    public void Out() => _state = false;

    public void InOut(Action? callback = null) => StartCoroutine(FadeInOutRoutine(callback)); 

    private IEnumerator FadeInOutRoutine(Action? callback)
    {
        In();
        yield return new WaitWhile(() => (1 - _canvasGroup.alpha) > 0.01f);
        callback?.Invoke();
        Out();
    }
}
