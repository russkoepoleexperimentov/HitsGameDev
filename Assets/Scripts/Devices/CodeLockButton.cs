using Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeLockButton : MonoBehaviour, IInteractable
{
    [SerializeField] private CodeLock _codeLock;
    [SerializeField] private int _digit;
    [SerializeField] private AudioClip _pressSound;

    public void OnInteraction()
    {
        AudioSource.PlayClipAtPoint(_pressSound, transform.position);
        _codeLock.EnterDigit(_digit);
    }

}
