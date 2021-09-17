using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ApplyCancelPanel : MonoBehaviour
{
    [SerializeField] private Button _applyButton;
    [SerializeField] private Button _cancelButton;

    public event UnityAction ApplyPressed;
    public event UnityAction CancelPressed;

    public void SetApplyInteraction(bool interactable)
    {
        _applyButton.interactable = interactable;
    }
    
    private void Awake()
    {
        _applyButton
            .onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                Debug.Log("ApplyPressed");
                ApplyPressed?.Invoke();
            })
            .AddTo(this);
        
        _cancelButton
            .onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                CancelPressed?.Invoke();
            })
            .AddTo(this);
    }
}
