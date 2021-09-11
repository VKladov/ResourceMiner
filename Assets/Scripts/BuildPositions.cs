using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildPositions : MonoBehaviour
{
    [SerializeField] private Grid _grid;

    private Dictionary<Vector2Int, Building> _buildings = new Dictionary<Vector2Int, Building>();

    public bool IsCellEmpty(Vector2Int cell)
    {
        return !_buildings.ContainsKey(cell);
    }

    public void PlaceBuilding(Building building, Vector2Int cell)
    {
        building.transform.position = _grid.GetWorldPosition(cell);
        _buildings[cell] = building;
    }
}
