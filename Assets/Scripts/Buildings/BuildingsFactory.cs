using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingsFactory : MonoBehaviour
{
    [SerializeField] private Building[] _buildings;
    
    private static BuildingsFactory _instance;
    public static BuildingsFactory Instance => _instance;
    public IEnumerable<Building> Buildings => _buildings;

    private void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;
    }

    public Building CreateBuilding(Building building)
    {
        var buildingView = Instantiate(building, Vector3.zero, Quaternion.identity);
        return buildingView;
    }
}
