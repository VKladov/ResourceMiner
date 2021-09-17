using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;

public class BuildHoveredPreview : MonoBehaviour
{
    [SerializeField] private float _moveDuration = 0.3f;
    
    private Building _building;
    private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
    private Vector3 _targetPosition;
    private void Awake()
    {
        MessageBus
            .Receive<ShowPreview>()
            .Subscribe(request =>
            {
                var newTargetPosition = request.Position + Vector3.up;
                if (newTargetPosition == _targetPosition && _building == request.Building)
                    return;

                _targetPosition = newTargetPosition;
                
                _cancellationTokenSource.Cancel();
                if (_building == null || _building != request.Building)
                {
                    _building = request.Building;
                    _building.transform.position = _targetPosition;
                }
                else
                {
                    _cancellationTokenSource = new CancellationTokenSource();
                    MoveToNewPosition(_targetPosition, _cancellationTokenSource.Token);
                }
            })
            .AddTo(this);
    }

    private async Task MoveToNewPosition(Vector3 targetPosition, CancellationToken cancellationToken)
    {
        var position = _building.transform.position;
        var time = 0f;
        var easing = new CubicBezier(BezierCurve.EaseInOut);
        while (time < _moveDuration && cancellationToken.IsCancellationRequested == false)
        {
            _building.transform.position = Vector3.Lerp(position, targetPosition, easing.GetValue(time / _moveDuration));
            time += Time.unscaledDeltaTime;
            await Task.Yield();
        }

        _building.transform.position = targetPosition;
    }

    public class ShowPreview : Request
    {
        public Vector3 Position;
        public Building Building;
    }
}
