using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel(string levelName)
    {
        var operation = new LoadingSceneOperation(levelName);
        LoadingScreen.Instance.AddToQueue(operation);
    }

    public void Exit() 
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
