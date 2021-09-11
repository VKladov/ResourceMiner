using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class BuildingSelectButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TMP_Text _text;
    [SerializeField] private RectTransform _priceContainer;
    [SerializeField] private ResourceAmountView _amountViewPrefab;

    private Building _data;

    private void Awake()
    {
        _button.onClick
            .AsObservable()
            .Subscribe(signal =>
            {
                new UIEvents.BuildingSelectButtonPressed
                {
                    Building = _data
                }.Publish();
            })
            .AddTo(this);
    }

    public void Show(Building building)
    {
        _data = building;
        _text.text = building.Label;
        
        foreach(Transform child in _priceContainer)
            Destroy(child.gameObject);

        foreach (var gameResourceAmount in building.Price)
        {
            var amountView = Instantiate(_amountViewPrefab, _priceContainer);
            amountView.Show(gameResourceAmount.Resource.Icon, gameResourceAmount.Amount);
        }
    }
}
