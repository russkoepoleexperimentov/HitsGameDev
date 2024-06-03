using UnityEngine;
using UnityEngine.Events;

public abstract class BaseActivator : MonoBehaviour
{
    [SerializeField] protected UnityEvent _activate;
    [SerializeField] protected UnityEvent _deactivate;
}
