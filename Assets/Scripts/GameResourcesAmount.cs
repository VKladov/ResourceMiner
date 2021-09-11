using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameResourcesAmount
{
    private readonly List<GameResourceAmount> _gameResources = new List<GameResourceAmount>();

    public event UnityAction<GameResource> Changed;

    public GameResourcesAmount(List<GameResourceAmount> gameResources)
    {
        _gameResources = gameResources;
    }
    
    public int GetResourceCount(GameResource resource)
    {
        return _gameResources.FirstOrDefault(item => item.Resource == resource)?.Amount ?? 0;
    }

    public void SetResourceCount(GameResource resource, int amount)
    {
        var resourceAmount = _gameResources.FirstOrDefault(item => item.Resource == resource);
        if (resourceAmount == null)
        {
            _gameResources.Add(new GameResourceAmount
            {
                Resource = resource,
                Amount = amount
            });
        }
        else
        {
            resourceAmount.Amount = amount;
        }
        
        Changed?.Invoke(resource);
    }
}
