using UnityEngine;

public class CollideSound : MonoBehaviour
{
    private const float VOLUME_THRESHOLD = 0.1f;

    [SerializeField] private AudioClip[] _clips;
    [SerializeField, Min(0.1f)] private float _referenceImpulse = 50f;

    private void OnCollisionEnter(Collision collision)
    {
        foreach(var contact in collision.contacts)
        {
            var volume = Mathf.Clamp01(contact.impulse.magnitude / _referenceImpulse);

            if (volume < VOLUME_THRESHOLD)
                continue;

            var clipIndex = Random.Range(0, _clips.Length);

            AudioSource.PlayClipAtPoint(_clips[clipIndex], contact.point, volume); 
        }

    }
}
