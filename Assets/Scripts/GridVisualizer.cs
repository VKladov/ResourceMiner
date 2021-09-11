using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private GameObject _linePrefab;


    public void Start()
    {
        var maxZ = _grid.GetWorldPosition(_grid.Size).z - _grid.CellSize * 0.5f;
        var minZ = _grid.GetWorldPosition(Vector2Int.zero).z - _grid.CellSize * 0.5f;
        var sizeZ = maxZ - minZ;
        var midZ = minZ + sizeZ * 0.5f;
        for (var x = 0; x < _grid.Size.x + 1; x++)
        {
            var lineX = _grid.GetWorldPosition(new Vector2Int(x, 0)).x - _grid.CellSize * 0.5f;
            var line = Instantiate(_linePrefab, transform);
            line.transform.position = new Vector3(lineX, 0, midZ);
            line.transform.localScale = new Vector3(1, 1, sizeZ);
            line.transform.rotation = Quaternion.LookRotation(Vector3.forward);
        }
        
        var maxX = _grid.GetWorldPosition(_grid.Size).x - _grid.CellSize * 0.5f;
        var minX = _grid.GetWorldPosition(Vector2Int.zero).x - _grid.CellSize * 0.5f;
        var sizeX = maxX - minX;
        var midX = minX + sizeX * 0.5f;
        for (var y = 0; y < _grid.Size.y + 1; y++)
        {
            var lineZ = _grid.GetWorldPosition(new Vector2Int(0, y)).z - _grid.CellSize * 0.5f;
            var line = Instantiate(_linePrefab, transform);
            line.transform.position = new Vector3(midX, 0, lineZ);
            line.transform.localScale = new Vector3(1, 1, sizeZ);
            line.transform.rotation = Quaternion.LookRotation(Vector3.right);
        }
    }
}
