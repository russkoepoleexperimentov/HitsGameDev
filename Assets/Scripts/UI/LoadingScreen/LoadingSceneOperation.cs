using System.Threading.Tasks;
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

    public async void Load()
    {
        var operation = SceneManager.LoadSceneAsync(_sceneName);
        while(!operation.isDone) 
            await Task.Delay(0);
        _done = true;
    }
}
