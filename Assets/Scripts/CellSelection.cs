using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class CellSelection : MonoBehaviour
{
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Grid _grid;
    [SerializeField] private float _maxPointerMove = 0.01f;
    
    private Camera _camera;
    private Vector2 _touchStartPoint;
    private bool _isTouchValid;
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            _touchStartPoint = Input.mousePosition;
            _isTouchValid = true;
        }

        if (Input.GetMouseButton(0))
        {
            if (Vector3.Distance(_touchStartPoint, Input.mousePosition) > _maxPointerMove * Screen.width)
                _isTouchValid = false;
        }

        if (Input.GetMouseButtonUp(0) && _isTouchValid)
        {
            if (TryGetCellUnderMouse(out var cell))
            {
                new CellSelected
                {
                    Cell = cell,
                    Grid = _grid
                }.Publish(); 
            }
        }
    }

    public bool TryGetCellInViewCenter(out Vector2Int cell)
    {
        var ray = new Ray(_camera.transform.position, _camera.transform.forward);
        if (Physics.Raycast(ray, out var hit, 1000, _groundLayer))
        {
            cell = _grid.GetNearestCell(hit.point);
            return true;
        }

        cell = Vector2Int.zero;
        return false;
    }

    public bool TryGetCellUnderMouse(out Vector2Int cell)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 1000, _groundLayer))
        {
            cell = _grid.GetNearestCell(hit.point);
            return true;
        }

        cell = Vector2Int.zero;
        return false;
    }

    public class CellSelected : Signal
    {
        public Vector2Int Cell;
        public Grid Grid;
    }
}
