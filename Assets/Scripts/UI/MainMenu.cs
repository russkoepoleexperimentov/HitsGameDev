using Assets.Scripts.General;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _continueButton;

    private void Start()
    {
        _continueButton.SetActive(Saver.HasSave());

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void NewGame()
    {
        Saver.ResetSave();
        LoadLevel(Saver.GetSavedLevel()); // если нет сохранения он вернет просто первый в игре уровень
    }

    public void ResetSave()
    {
        Saver.ResetSave();
        _continueButton.SetActive(false);
    }

    public void ContinueGame() => LoadLevel(Saver.GetSavedLevel());

    private void LoadLevel(string levelName)
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
