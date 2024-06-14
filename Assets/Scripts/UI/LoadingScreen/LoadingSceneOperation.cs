using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneOperation : ILoadingOperation
{
    private readonly string _sceneName;
    private bool _done = false;

    public bool Done => _done;

    public LoadingSceneOperation(string sceneName)
    {
        _sceneName = sceneName;
    }

    public void Load()
    {
        var operation = SceneManager.LoadSceneAsync(_sceneName);

        operation.completed += (_) =>
        {
            _done = true;
        };
    }
}
