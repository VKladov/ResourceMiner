using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildSelectionView : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private BuildingSelectButton _buttonPrefab;

    private void Awake()
    {
        foreach (Transform child in _container)
            Destroy(child.gameObject);

        foreach (var buildingType in BuildingsFactory.Instance.Buildings)
        {
            var button = Instantiate(_buttonPrefab, _container);
            button.Show(buildingType);
        }
    }
}
