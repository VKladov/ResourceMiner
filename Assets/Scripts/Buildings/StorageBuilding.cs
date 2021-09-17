using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageBuilding : Building
{
    [SerializeField] private GameResourceAmount _capacity;

    public GameResourceAmount Capacity => _capacity;
    public override void StartWork()
    {
        
    }

    public override string GetDescription()
    {
        return $"Allows you to store {_capacity.Amount} more units of {_capacity.Resource.name}";
    }
}
