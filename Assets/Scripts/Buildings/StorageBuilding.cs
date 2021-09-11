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
}
