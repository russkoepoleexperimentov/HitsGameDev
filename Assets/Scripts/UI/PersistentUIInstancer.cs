using UnityEngine;

public static class PersistentUIInstancer
{
    private const string GENERAL_UI = "prefabs/ui/uicanvas";
    private const string LOADING_UI = "prefabs/ui/loadingscreencanvas";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void CreateUI()
    {
        Instantiate(GENERAL_UI);
        Instantiate(LOADING_UI);
    }

    private static void Instantiate(string path)
    {
        var res = Resources.Load(path) as GameObject;
        var instance = Object.Instantiate(res, Vector3.zero, Quaternion.identity);
        Object.DontDestroyOnLoad(instance);
    }
}
