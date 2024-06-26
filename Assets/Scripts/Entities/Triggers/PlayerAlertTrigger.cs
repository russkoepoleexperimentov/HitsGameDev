using UnityEngine;

public class PlayerAlertTrigger : MonoBehaviour
{
    [SerializeField, Multiline(5)] private string _message;
    [SerializeField] private float _duration;
    [SerializeField] private bool _selfDestroy = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            AlertPopup.Instance.Show(_message, _duration);

            if (_selfDestroy) Destroy(gameObject);
        }
    }
}
