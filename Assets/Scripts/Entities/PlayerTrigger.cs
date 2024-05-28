using UnityEngine;
using UnityEngine.Events;

public class PlayerTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _onPlayerEnter;
    [SerializeField] private bool _selfDestroy = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _onPlayerEnter.Invoke();
            if(_selfDestroy)
                Destroy(gameObject);
        }
    }
}
