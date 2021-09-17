using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UniRx;
using UnityEngine;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private BuildPositions _buildPositions;
    [SerializeField] private CellSelection _cellSelection;
    [SerializeField] private ApplyCancelPanel _applyCancelPanel;

    private Building _building;
    private Vector2Int _selectedCell;

    public void StartPlacing(Building building)
    {
        _building = building;
        _applyCancelPanel.gameObject.SetActive(true);
        _applyCancelPanel.ApplyPressed += OnApplyPressed;
        _applyCancelPanel.CancelPressed += OnCancelPressed;
        
        if (!_cellSelection.TryGetCellInViewCenter(out var cellInViewCenter)) return;
        if (!_buildPositions.TryGetNearestEmptyCell(cellInViewCenter, out var emptyCell)) return;
        new BuildingEvents.ShowGridRequest().Publish();
                    
        new CellSelection.CellSelected
        {
            Cell = emptyCell
        }.Publish();
        
        _cellSelection.enabled = true;
    }

    public void FinishPlacing()
    {
        _applyCancelPanel.gameObject.SetActive(false);
        _applyCancelPanel.ApplyPressed -= OnApplyPressed;
        _applyCancelPanel.CancelPressed -= OnCancelPressed;
        
        new CellSelectionView.HideCellHighlight().Publish();
        new BuildingEvents.HideGridRequest().Publish();
        
        _cellSelection.enabled = false;
    }

    private void Awake()
    {
        _cellSelection.enabled = false;
        
        MessageBus
            .Receive<CellSelection.CellSelected>()
            .Subscribe(signal =>
            {
                _selectedCell = signal.Cell;
                var position = _grid.GetWorldPosition(signal.Cell);
                var valid = _buildPositions.IsCellEmpty(signal.Cell);
                
                new CellSelectionView.HighlightCell
                {
                    Position = position,
                    IsValid = valid
                }.Publish();
                
                new BuildHoveredPreview.ShowPreview
                {
                    Position = position,
                    Building = _building
                }.Publish();
                
                _applyCancelPanel.SetApplyInteraction(valid);
            })
            .AddTo(this);
    }

    private void OnApplyPressed()
    {
        if (!_buildPositions.IsCellEmpty(_selectedCell)) return;
        
        new UIEvents.PlaceBuilding
        {
            Building = _building,
            Cell = _selectedCell
        }.Publish();
        
        FinishPlacing();
    }

    private void OnCancelPressed()
    {
        Destroy(_building.gameObject);
        FinishPlacing();
    }
}
