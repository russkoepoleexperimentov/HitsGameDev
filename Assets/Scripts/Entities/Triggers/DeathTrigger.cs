using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    //[SerializeField] private Transform _safeSpot;
    [SerializeField] private float _damage = 100;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<TestChamberCube>(out var cube))
        {
            cube.AttachedDispenser.SpawnNew();
            cube.Remove();
        }

        if (other.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(_damage);
            /*ScreenFade.Singleton.InOut(() =>
            {
                other.transform.position = _safeSpot.position;
            });*/
        }
    }
}
