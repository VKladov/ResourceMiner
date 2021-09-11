using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class TimeScaleControlPanel : MonoBehaviour
{
    [SerializeField] private Button _x1Button;
    [SerializeField] private Button _x2Button;
    [SerializeField] private Button _x3Button;

    private void Awake()
    {
        _x1Button
            .onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                Time.timeScale = 1;
            })
            .AddTo(this);
        
        _x2Button
            .onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                Time.timeScale = 2;
            })
            .AddTo(this);
        
        _x3Button
            .onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                Time.timeScale = 3;
            })
            .AddTo(this);
    }
}
