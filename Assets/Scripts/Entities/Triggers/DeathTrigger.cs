using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<TestChamberCube>(out var cube))
        {
            cube.AttachedDispenser.SpawnNew();
            cube.Remove();
        }
    }
}
