using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private GameObject _loadingElements; 
    [SerializeField] private GameObject _keyPressElements; 

    private Queue<ILoadingOperation> _operationQueue;
    private bool _loadingEnabled = false;

    public static LoadingScreen Instance { get; private set; }  

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if(Instance == null)
            Instance = this;
        else
            Destroy(Instance);

        _operationQueue = new Queue<ILoadingOperation>();

        HideAll();
    }

    public void AddToQueue(ILoadingOperation operation)
    {
        _operationQueue.Enqueue(operation);

        if(!_loadingEnabled)
        {
            StartCoroutine(LoadQueue());
            _loadingEnabled = true;
        }

    }

    private IEnumerator LoadQueue()
    {
        InitLoading();
        while(_operationQueue.Count > 0)
        {
            var thing = _operationQueue.Dequeue();
            thing.Load();
            yield return new WaitWhile(() => thing.Done == false);
        }
        HideAll();
    }

    private void InitLoading()
    {
        _canvas.enabled = true;
        _group.alpha = 1.0f;
        _loadingElements.SetActive(true);
        _keyPressElements.SetActive(false);
    }

    private void InitKeyWaiting()
    {
        _loadingElements.SetActive(false);
        _keyPressElements.SetActive(true);
    }

    private void HideAll()
    {
        _canvas.enabled = false;
        _group.alpha = 0;
    }
}

public interface ILoadingOperation
{
    void Load();
    bool Done { get; }
}
