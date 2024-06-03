using UnityEngine;

public class CubeDispenser : MonoBehaviour
{
    [SerializeField] private TestChamberCube _prefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private bool _spawnOnStart = true;

    private TestChamberCube _current;

    private void Start()
    {
        if(_spawnOnStart)
            SpawnNew(); 
    }

    public void SpawnNew()
    {
        if(_current != null)
            _current.Remove();

        _current = Instantiate(_prefab, _spawnPoint.position, _spawnPoint.rotation);
        _current.AttachedDispenser = this;
    }
}
