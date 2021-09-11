using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;
    [SerializeField] private float _cellSize;

    public Vector2Int Size => _size;
    public float CellSize => _cellSize;

    private Vector3 TotalSize => new Vector3(_size.x * _cellSize, 0, _size.y * _cellSize);

    public Vector3 GetWorldPosition(Vector2Int cell)
    {
        return new Vector3(_cellSize * cell.x, 0, _cellSize * cell.y) - TotalSize * 0.5f;
    }

    public Vector2Int GetNearestCell(Vector3 worldPosition)
    {
        Vector3 localPosition = worldPosition + TotalSize * 0.5f;
        return new Vector2Int(
            Mathf.FloorToInt((localPosition.x + _cellSize * 0.5f) / _cellSize),
            Mathf.FloorToInt((localPosition.z + _cellSize * 0.5f) / _cellSize)
            );
    }
}
