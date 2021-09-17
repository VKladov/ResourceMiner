using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class CellSelectionView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _okColor;
    [SerializeField] private Color _errorColor;

    private void Awake()
    {
        _spriteRenderer.enabled = false;
        
        MessageBus
            .Receive<HighlightCell>()
            .Subscribe(request =>
            {
                transform.position = request.Position;
                _spriteRenderer.enabled = true;
                _spriteRenderer.color = request.IsValid ? _okColor : _errorColor;
            })
            .AddTo(this);
        
        MessageBus
            .Receive<HideCellHighlight>()
            .Subscribe(request =>
            {
                _spriteRenderer.enabled = false;
            })
            .AddTo(this);
    }

    public class HighlightCell : Request
    {
        public Vector3 Position;
        public bool IsValid;
    }

    public class HideCellHighlight : Request {}
}
