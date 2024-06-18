using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class AlertPopup : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Text _label;
    [SerializeField] private AudioClip _clip;
     
    private const string ANM_TRIG_SHOW = "show";
    private const string ANM_TRIG_HIDE = "hide";
    private const float HIDE_DELAY = 0.5f;

    private Queue<KeyValuePair<string, float>> _messages = new();

    private AudioSource _source;

    public static AlertPopup Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        StartCoroutine(MessageLoop());
    }

    public void Show(string text, float duration = 5) => _messages.Enqueue(new(text, duration));
    public void Show(string text) => Show(text, 5);

    private IEnumerator MessageLoop()
    {
        while(true)
        {
            while(_messages.Count > 0)
            {
                var message = _messages.Dequeue();

                _source.PlayOneShot(_clip);

                _label.text = message.Key;

                _animator.SetTrigger(ANM_TRIG_SHOW);

                yield return new WaitForSeconds(message.Value);
                
                _animator.SetTrigger(ANM_TRIG_HIDE);

                yield return new WaitForSeconds(HIDE_DELAY);
            }

            yield return null;
        }
    }
}
