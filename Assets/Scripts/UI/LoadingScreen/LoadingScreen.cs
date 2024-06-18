using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private CanvasGroup _group;
    [SerializeField] private GameObject _loadingElements;

    private const float _fadeTime = 0.3f;

    private Queue<ILoadingOperation> _operationQueue;
    private bool _loadingEnabled = false;

    public static LoadingScreen Instance { get; private set; }  

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        _operationQueue = new Queue<ILoadingOperation>();

        _canvas.enabled = false;
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
        _canvas.enabled = true;
        _group.alpha = 0f;

        for (float t = 0; t <= 1f; t += Time.deltaTime / _fadeTime)
        {
            _group.alpha = t;
            yield return null;
        }

        _group.alpha = 1.0f;

        while (_operationQueue.Count > 0)
        {
            var thing = _operationQueue.Dequeue();
            thing.Load();

            while(thing.Done == false)
            {
                yield return null;
            }
        }
        yield return new WaitForSeconds(1f);

        for (float t = 0; t <= 1f; t += Time.deltaTime / _fadeTime) 
        { 
            _group.alpha = 1 - t;
            yield return null;
        }

        _group.alpha = 0f;

        _canvas.enabled = false;
    }
}

public interface ILoadingOperation
{
    void Load();
    bool Done { get; }
}
