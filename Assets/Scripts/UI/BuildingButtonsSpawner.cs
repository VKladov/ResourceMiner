using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx;
using UnityEngine;

public class BuildingButtonsSpawner : MonoBehaviour
{
    [SerializeField] private ProgressIcon _progressIconPrefab;
    [SerializeField] private Sprite _buildingIcon;
    [SerializeField] private BuildingResourceTakeButton _takeButtonPrefab;

    private Camera _camera;
    private List<ProgressIcon> _icons = new List<ProgressIcon>();
    private List<BuildingResourceTakeButton> _buttons = new List<BuildingResourceTakeButton>();
    private void Awake()
    {
        _camera = Camera.main;
        
        MessageBus
            .Receive<BuildingEvents.StartedBuild>()
            .Subscribe(signal =>
            {
                CreateIcon(signal.Building, _buildingIcon);
            })
            .AddTo(this);
        
        MessageBus
            .Receive<BuildingEvents.StartedExtraction>()
            .Subscribe(signal =>
            {
                CreateIcon(signal.Building, signal.Building.ExtractingResource.Resource.Icon);
            })
            .AddTo(this);

        MessageBus
            .Receive<BuildingEvents.FinishedBuilding>()
            .Subscribe(signal =>
            {
                RemoveIcon(signal.Building);
            })
            .AddTo(this);
        
        MessageBus
            .Receive<BuildingEvents.FinishedExtraction>()
            .Subscribe(signal =>
            {
                RemoveIcon(signal.Building);
            })
            .AddTo(this);
        
        MessageBus
            .Receive<BuildingEvents.ExtractedResourcesChanged>()
            .Subscribe(signal =>
            {
                RemoveButton(signal.Building);
                if (signal.Building.ExtractedResource.Amount > 0)
                    CreateButton(signal.Building);
            })
            .AddTo(this);
    }

    private void RemoveIcon(Building building)
    {
        var icon = _icons.FirstOrDefault(item => item.Building == building);
        if (!icon) return;
        _icons.Remove(icon);
        Destroy(icon.gameObject);
    }

    private void RemoveButton(Building building)
    {
        var icon = _buttons.FirstOrDefault(item => item.Building == building);
        if (!icon) return;
        _buttons.Remove(icon);
        Destroy(icon.gameObject);
    }

    private void CreateIcon(Building building, Sprite sprite)
    {
        var icon = Instantiate(_progressIconPrefab, transform);
        icon.Init(building, sprite, _camera);
        _icons.Add(icon);
    }

    private void CreateButton(MiningBuilding building)
    {
        var button = Instantiate(_takeButtonPrefab, transform);
        button.Init(building, _camera);
        _buttons.Add(button);
    }
}
