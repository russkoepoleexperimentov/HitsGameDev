using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TestChamberCube : MonoBehaviour
{
    [SerializeField] private AudioClip _removeClip;
    private Renderer _renderer;
    private Rigidbody _rigidbody;
    private Collider _collider;
    private bool _isDestroying = false;

    public CubeDispenser AttachedDispenser { get; set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
    }

    public void Remove() 
    { 
        if(_isDestroying) return;

        _isDestroying = true;
        StartCoroutine(Destroy()); 
    }

    private IEnumerator Destroy()
    {
        _rigidbody.useGravity = false;
        _rigidbody.velocity *= 0.4f;
        Destroy(_collider);

        AudioSource.PlayClipAtPoint(_removeClip, transform.position);

        for (float t = 0; t <= 1; t += Time.deltaTime)
        {
            _renderer.material.color = Color.Lerp(Color.white, Color.black, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}
