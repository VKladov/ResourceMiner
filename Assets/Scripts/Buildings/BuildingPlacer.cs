using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Grid _grid;

    private Camera _camera;
    private Building _buildingToPlace;
    private Vector2Int _currentCell;
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!_buildingToPlace)
            return;
        
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000, _groundLayer))
        {
            _currentCell = _grid.GetNearestCell(hit.point);
            _buildingToPlace.transform.position = _grid.GetWorldPosition(_currentCell);
        }

        if (Input.GetMouseButtonDown(0))
        {
            new UIEvents.PlaceBuilding
            {
                Building = _buildingToPlace,
                Cell = _currentCell
            }.Publish();
        }
    }

    public void SetBuilding(Building building)
    {
        _buildingToPlace = building;
    }
}
