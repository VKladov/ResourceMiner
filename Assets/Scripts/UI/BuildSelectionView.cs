using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BuildSelectionView : MonoBehaviour
{
    [SerializeField] private RectTransform _container;
    [SerializeField] private BuildingSelectButton _buttonPrefab;
    [SerializeField] private Button _closeButton;

    private void Awake()
    {
        foreach (Transform child in _container)
            Destroy(child.gameObject);

        foreach (var buildingType in BuildingsFactory.Instance.Buildings)
        {
            var button = Instantiate(_buttonPrefab, _container);
            button.Show(buildingType);
        }

        _closeButton
            .onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                gameObject.SetActive(false);
            })
            .AddTo(this);
    }
}
