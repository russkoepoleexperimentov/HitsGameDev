using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            var operation = new LoadingSceneOperation(_nextSceneName);
            LoadingScreen.Instance.AddToQueue(operation);
        }
    }
}
