using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BuildingStatusButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private Image _progressImage;
    [SerializeField] private Image _statusImage;
    [SerializeField] private Sprite _buildingSprite;

    private Building _building;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        _button
            .onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                new UIEvents.TakeBuildingResources
                {
                    Building = _building
                }.Publish();
            })
            .AddTo(this);
    }

    public void Init(Building building)
    {
        _building = building;
    }

    private void Update()
    {
        if (!_building)
            return;

        transform.position = _camera.WorldToScreenPoint(_building.transform.position + Vector3.up);
    }
}
