using UnityEngine;

public class DamageIndicator : MonoBehaviour
{
    [SerializeField] private float _fadeOutSpeed = 1.5f;
    [SerializeField] private CanvasGroup _canvasGroup;

    public static DamageIndicator Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if(_canvasGroup.alpha > 0)
            _canvasGroup.alpha -= Time.deltaTime * _fadeOutSpeed;
    }

    public void Trigger() => _canvasGroup.alpha = 1;
}
