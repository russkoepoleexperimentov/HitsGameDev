using Assets.Scripts.General;
using UnityEngine;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if (Saver.UpdateSave(_nextSceneName))
                AlertPopup.Instance.Show("���� ���������", 2);

            var operation = new LoadingSceneOperation(_nextSceneName);
            LoadingScreen.Instance.AddToQueue(operation);
        }
    }
}
