using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ProgressIcon : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Image _progressImage;

    public Building Building { get; private set; }
    private Camera _camera;
    
    public void Init(Building building, Sprite icon, Camera mainCamera)
    {
        Building = building;
        _icon.sprite = icon;
        _camera = mainCamera;
        Update();
    }
    
    private void Update()
    {
        transform.position = _camera.WorldToScreenPoint(Building.transform.position + Vector3.up * 0.5f);
        _progressImage.fillAmount = Building.Progress;
    }
}
