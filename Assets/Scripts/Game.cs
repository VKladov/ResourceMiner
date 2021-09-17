using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField] private Button _buildButton;
    [SerializeField] private BuildSelectionView _buildSelection;
    [SerializeField] private BuildingPlacer _buildingPlacer;
    [SerializeField] private BuildPositions _buildPositions;
    [SerializeField] private BuildingsFactory _buildingsFactory;

    private Building _buildingToPlace;
    
    private void Awake()
    {
        #if UNITY_EDITOR
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        #endif

        _buildButton
            .onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                if (_buildSelection.gameObject.activeSelf)
                {
                    _buildSelection.gameObject.SetActive(false);
                }
                else
                {
                    _buildSelection.gameObject.SetActive(true);
                }
            })
            .AddTo(this);

        MessageBus
            .Receive<UIEvents.PlaceBuilding>()
            .Subscribe(request =>
            {
                if (_buildPositions.IsCellEmpty(request.Cell) && CanPlayerBuyBuilding(_buildingToPlace))
                {
                    SpendResources(_buildingToPlace.Price.ToList());
                    _buildPositions.PlaceBuilding(request.Building, request.Cell);
                    _buildingPlacer.FinishPlacing();
                    
                    request.Building.Build();
                    
                    new BuildingEvents.HideGridRequest().Publish();
                    new CellSelectionView.HideCellHighlight().Publish();
                }
            })
            .AddTo(this);
        
        MessageBus
            .Receive<UIEvents.BuildingSelectButtonPressed>()
            .Subscribe(request =>
            {
                if (!CanPlayerBuyBuilding(request.Building)) return;
                _buildSelection.gameObject.SetActive(false);
                _buildingToPlace = request.Building;
                var building = _buildingsFactory.CreateBuilding(request.Building);
                _buildingPlacer.StartPlacing(building);
            })
            .AddTo(this);

        MessageBus
            .Receive<BuildingEvents.TakeResourcesRequest>()
            .Subscribe(request =>
            {
                var resource = request.Building.ExtractingResource.Resource;
                request.Building.TakeResource(GetAllowedCapacity(resource), out var amountTaken);
                AddResources(resource, amountTaken);
            })
            .AddTo(this);

        MessageBus
            .Receive<BuildingEvents.FinishedBuilding>()
            .Subscribe(request =>
            {
                if (request.Building is StorageBuilding building)
                {
                    AddCapacity(building.Capacity);
                }
            })
            .AddTo(this);
    }

    private static void AddCapacity(GameResourceAmount resourceAmount)
    {
        var currentCapacity = PlayerState.Instance.ResourcesCapacity.GetResourceCount(resourceAmount.Resource);
        PlayerState.Instance.ResourcesCapacity.SetResourceCount(resourceAmount.Resource, currentCapacity + resourceAmount.Amount);
    }

    private static bool CanPlayerBuyBuilding(IBuildable building)
    {
        foreach (var resourceCost in building.Price)
            if (resourceCost.Amount > PlayerState.Instance.ResourcesAvailable.GetResourceCount(resourceCost.Resource))
                return false;
        
        return true;
    }

    private static void SpendResources(IEnumerable<GameResourceAmount> resourceAmounts)
    {
        foreach (var resourceCost in resourceAmounts)
        {
            var available = PlayerState.Instance.ResourcesAvailable.GetResourceCount(resourceCost.Resource);
            PlayerState.Instance.ResourcesAvailable.SetResourceCount(resourceCost.Resource, available - resourceCost.Amount);
        }
    }

    private static void AddResources(GameResource resource, int amount)
    {
        if (amount == 0)
            return;
        
        var available = PlayerState.Instance.ResourcesAvailable.GetResourceCount(resource);
        PlayerState.Instance.ResourcesAvailable.SetResourceCount(resource, available + amount);
    }

    private static int GetAllowedCapacity(GameResource resource)
    {
        var capacity = PlayerState.Instance.ResourcesCapacity.GetResourceCount(resource);
        var available = PlayerState.Instance.ResourcesAvailable.GetResourceCount(resource);
        return capacity - available;
    }
}
