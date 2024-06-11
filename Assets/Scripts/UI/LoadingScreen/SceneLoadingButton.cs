using UnityEngine;

public class SceneLoadingButton : MonoBehaviour
{
    public void Load(string name)
    {
        LoadingScreen.Instance.AddToQueue(new LoadingSceneOperation(name));
    }
}
