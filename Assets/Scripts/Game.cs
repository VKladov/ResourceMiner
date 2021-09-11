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
                    _buildingPlacer.enabled = false;
                    request.Building.Build();
                }
            })
            .AddTo(this);
        
        MessageBus
            .Receive<UIEvents.BuildingSelectButtonPressed>()
            .Subscribe(request =>
            {
                if (CanPlayerBuyBuilding(request.Building))
                {
                    _buildingToPlace = request.Building;
                    var building = _buildingsFactory.CreateBuilding(request.Building);
                    _buildingPlacer.SetBuilding(building);
                    _buildingPlacer.enabled = true;
                }
            })
            .AddTo(this);

        MessageBus
            .Receive<BuildingEvents.TakeResourcesRequest>()
            .Subscribe(request =>
            {
                if (!CanStoreResources(request.Building.ExtractingResource)) return;
                
                AddResources(request.Building.ExtractingResource);
                request.Building.Clear();
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

    private static void AddResources(GameResourceAmount resourceAmount)
    {
        var available = PlayerState.Instance.ResourcesAvailable.GetResourceCount(resourceAmount.Resource);
        PlayerState.Instance.ResourcesAvailable.SetResourceCount(resourceAmount.Resource, available + resourceAmount.Amount);
    }

    private static bool CanStoreResources(GameResourceAmount resourceAmount)
    {
        var capacity = PlayerState.Instance.ResourcesCapacity.GetResourceCount(resourceAmount.Resource);
        var available = PlayerState.Instance.ResourcesAvailable.GetResourceCount(resourceAmount.Resource);
        return available + resourceAmount.Amount <= capacity;
    }
}
