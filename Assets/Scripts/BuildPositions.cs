using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildPositions : MonoBehaviour
{
    [SerializeField] private Grid _grid;

    private readonly Dictionary<Vector2Int, Building> _buildings = new Dictionary<Vector2Int, Building>();

    public bool IsCellEmpty(Vector2Int cell)
    {
        return !_buildings.ContainsKey(cell);
    }

    public bool TryGetNearestEmptyCell(Vector2Int cell, out Vector2Int result)
    {
        result = cell;
        var radius = 0;
        var emptyCells = new List<Vector2Int>();
        while (!emptyCells.Any())
        {
            var cellsAround = GetCellsAround(cell, radius);
            if (!cellsAround.Any())
                return false;

            emptyCells = cellsAround.Where(IsCellEmpty).ToList();
            
            radius++;
        }
        
        result = emptyCells.First();
        return true;
    }

    public List<Vector2Int> GetCellsAround(Vector2Int cell, int radius)
    {
        if (radius == 0)
            return new List<Vector2Int>{ cell };

        var zero = cell - new Vector2Int(radius, radius);
        var result = new List<Vector2Int>();
        var cols = radius * 2 + 1;
        for (var x = 0; x < cols; x++)
        {
            if (x == 0 || x == cols - 1)
            {
                for (var y = 0; y < cols; y++)
                {
                    result.Add(new Vector2Int(zero.x + x, zero.y + y));
                }
            }
            else
            {
                result.Add(new Vector2Int(zero.x + x, zero.y));
                result.Add(new Vector2Int(zero.x + x, zero.y + cols - 1));
            }
        }

        return result;
    }

    public void PlaceBuilding(Building building, Vector2Int cell)
    {
        building.transform.position = _grid.GetWorldPosition(cell);
        _buildings[cell] = building;
    }
}
