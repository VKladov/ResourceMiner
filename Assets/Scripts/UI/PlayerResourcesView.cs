using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourcesView : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private ResourceStateView _stateViewPrefab;

    private void Awake()
    {
        foreach (Transform child in _container)
            Destroy(child.gameObject);

        foreach (var resource in GameResource.All)
        {
            var stateView = Instantiate(_stateViewPrefab, _container);
            stateView.Init(resource);
        }
    }
}
