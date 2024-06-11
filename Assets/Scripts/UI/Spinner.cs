using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField] private Vector3 _spin = Vector3.forward * 180f;

    private void Update()
    {
        transform.Rotate(_spin * Time.deltaTime);
    }
}
