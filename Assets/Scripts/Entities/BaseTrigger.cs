using UnityEngine;
using UnityEngine.Events;

public abstract class BaseTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _onEnter;
    [SerializeField] private UnityEvent _onExit;
    [SerializeField] private bool _selfDestroy = false;

    public UnityEvent OnEnter => _onEnter;
    public UnityEvent OnExit => _onExit;

    protected void CallOnEnter()
    {
        _onEnter?.Invoke();
        if (_selfDestroy) Destroy(gameObject);
    }
    protected void CallOnExit()
    {
        _onExit?.Invoke();
    }

    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void OnTriggerExit(Collider other);
}
