using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] private Transform _safeSpot;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<TestChamberCube>(out var cube))
        {
            cube.AttachedDispenser.SpawnNew();
            cube.Remove();
        }

        if (other.tag == "Player")
        {
            ScreenFade.Singleton.InOut(() =>
            {
                other.transform.position = _safeSpot.position;
            });
        }
    }
}
