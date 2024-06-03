using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomPitcher : MonoBehaviour
{
    [SerializeField] private float _timeBetweenPitch = 2;
    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;

    private AudioSource _source;
    private float _timer;
    private float _pitch;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_timer < 0)
        {
            _pitch = Random.Range(_minPitch, _maxPitch);
            _timer = Random.Range(0f, _timeBetweenPitch);
        }
        else
        {
            _timer -= Time.deltaTime;
        }

        _source.pitch = Mathf.MoveTowards(_source.pitch, _pitch, Time.deltaTime);
    }
}
