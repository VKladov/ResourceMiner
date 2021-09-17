using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BuildingResourceTakeButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private ResourceAmountView _amountView;

    public MiningBuilding Building { get; private set; }
    private Camera _camera;

    public void Init(MiningBuilding building, Camera mainCamera)
    {
        Building = building;
        _amountView.Show(building.ExtractedResource.Resource.Icon, building.ExtractedResource.Amount);
        _camera = mainCamera;
        Update();
    }
    
    private void Awake()
    {
        _button
            .onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                new BuildingEvents.TakeResourcesRequest
                {
                    Building = Building
                }.Publish();
            })
            .AddTo(this);
    }
    
    private void Update()
    {
        transform.position = _camera.WorldToScreenPoint(Building.transform.position + Vector3.up * 0.5f);
    }
}
