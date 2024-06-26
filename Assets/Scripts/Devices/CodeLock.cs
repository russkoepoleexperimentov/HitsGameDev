using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Linq;

public class CodeLock : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private UnityEvent _onCorrectCode;
    [SerializeField] private UnityEvent _onIncorrectCode;
    [SerializeField] private List<int> _correctCode;
    [SerializeField] private AudioClip _incorrectCodeSound;
    [SerializeField] private AudioClip _correctCodeSound;

    private List<int> _entered;
    private void Start()
    {
        _entered = new List<int>();
        _text.text = "";
    }

    public void EnterDigit(int num)
    {
        if (_entered.Count < _correctCode.Count)
        {
            _entered.Add(num);
            DisplayEntered();
        }
        if (_entered.Count == _correctCode.Count)
        {
            var codeStr = string.Join("", _entered.ToArray());
            var correctCodeStr = string.Join("", _correctCode.ToArray());

            if (correctCodeStr != codeStr)
            {
                AudioSource.PlayClipAtPoint(_incorrectCodeSound, transform.position);
                StartCoroutine(ShowText("ERR"));
                _onIncorrectCode?.Invoke();
            }
            else
            {
                AudioSource.PlayClipAtPoint(_correctCodeSound, transform.position);
                StartCoroutine(ShowText("CORR"));
                _onCorrectCode?.Invoke();
            }
        }
    }

    private IEnumerator ShowText(string text)
    {
        yield return new WaitForSeconds(0.1f);
        _entered.Clear();
        _text.text = text;
        yield return new WaitForSeconds(0.5f);
        DisplayEntered();
    }

    private void DisplayEntered()
    {
        var codeDispl = string.Join("", _entered.ToArray());
        _text.text = codeDispl;
    }
}
