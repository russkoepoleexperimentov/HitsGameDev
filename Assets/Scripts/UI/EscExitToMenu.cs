using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscExitToMenu : MonoBehaviour
{
    [SerializeField] private Image _progressCircle;

    public const string MENU_SCENE = "_main_menu";
    private const float DELAY = 2;
    private const float POPUP_DELAY = 5;

    private float _nextPopupTime;

    private void Start()
    {
        _progressCircle.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != MENU_SCENE) 
            StartCoroutine(DelayExit());
    }

    private IEnumerator DelayExit()
    {
        _progressCircle.enabled = true;

        for (float t = 0; t <= DELAY; t += Time.deltaTime)
        {
            if(!Input.GetKey(KeyCode.Escape))
            {
                if (Time.time > _nextPopupTime)
                {
                    AlertPopup.Instance.Show("Зажмите [ESC] чтобы выйти", 1.5f);
                    _nextPopupTime = Time.time + POPUP_DELAY;
                }
                _progressCircle.enabled = false;
                yield break;
            }

            _progressCircle.fillAmount = t / DELAY;

            yield return null;
        }

        _progressCircle.enabled = false;

        var operation = new LoadingSceneOperation(MENU_SCENE);
        LoadingScreen.Instance.AddToQueue(operation);
    }
}
