using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class CodeLock : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] protected UnityEvent _activate;
    [SerializeField] private List<int> _correctCode;
    [SerializeField] private AudioClip _incorrectCodeSound;
    [SerializeField] private AudioClip _correctCodeSound;

    private List<int> _entered;
    private void Start()
    {
        _entered = new List<int>();
    }

    public void EnterDigit(int num)
    {
        if (_entered.Count < _correctCode.Count)
        {
            _text.text += num.ToString();
            _entered.Add(num);
        }
        if (_entered.Count == _correctCode.Count)
        {
            if (_entered != _correctCode)
            {
                AudioSource.PlayClipAtPoint(_incorrectCodeSound, transform.position);
                _text.text = "";
                _entered.Clear();
            }

            else
            {
                _activate?.Invoke();
                AudioSource.PlayClipAtPoint(_correctCodeSound, transform.position);
            }
        }
    }

}
