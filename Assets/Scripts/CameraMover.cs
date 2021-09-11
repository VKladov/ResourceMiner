using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _raycastTarget;

    private Vector3 _anchorPoint;
    private Vector3 _cameraVelocity;
    private Vector3 _targetCameraPoint;

    private void Awake()
    {
        _targetCameraPoint = _camera.transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, _raycastTarget))
                _anchorPoint = hit.point;
        }

        if (Input.GetMouseButton(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000, _raycastTarget))
                _targetCameraPoint = _anchorPoint + (_camera.transform.position - hit.point);
        }

        _camera.transform.position = Vector3.SmoothDamp(_camera.transform.position, _targetCameraPoint, ref _cameraVelocity, 0.1f);
    }
}
