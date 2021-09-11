using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerState
{
    private static PlayerState _instance;
    
    public static PlayerState Instance
    {
        get
        {
            if (_instance == null)
                _instance = new PlayerState(
                    new List<GameResourceAmount>
                    {
                        new GameResourceAmount
                        {
                            Resource = GameResource.Mineral,
                            Amount = 150
                        },
                        new GameResourceAmount
                        {
                            Resource = GameResource.Gas,
                            Amount = 150
                        },
                    },
                    new List<GameResourceAmount>
                    {
                        new GameResourceAmount
                        {
                            Resource = GameResource.Mineral,
                            Amount = 150
                        },
                        new GameResourceAmount
                        {
                            Resource = GameResource.Gas,
                            Amount = 150
                        },
                    });

            return _instance;
        }
    }
    
    public readonly GameResourcesAmount ResourcesAvailable;
    public readonly GameResourcesAmount ResourcesCapacity;

    public PlayerState(List<GameResourceAmount> startResources, List<GameResourceAmount> startCapacity)
    {
        ResourcesAvailable = new GameResourcesAmount(startResources);
        ResourcesCapacity = new GameResourcesAmount(startCapacity);

        ResourcesAvailable.Changed += AvailableResourcesChanged;
        ResourcesCapacity.Changed += ResourcesCapacityChanged;
    }

    private void AvailableResourcesChanged(GameResource resource)
    {
        new PlayerStateEvents.ResourceAmountChanged
        {
            Resource = resource
        }.Publish();
    }

    private void ResourcesCapacityChanged(GameResource resource)
    {
        new PlayerStateEvents.ResourceAmountChanged
        {
            Resource = resource
        }.Publish();
    }
}
