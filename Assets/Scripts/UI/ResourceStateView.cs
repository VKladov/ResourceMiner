using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStateView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private TMP_Text _label;
    [SerializeField] private Image _image;

    private GameResource _resource;
    public void Init(GameResource resource)
    {
        _resource = resource;
        _image.sprite = resource.Icon;
        UpdateState();

        MessageBus
            .Receive<PlayerStateEvents.ResourceAmountChanged>()
            .Subscribe(signal =>
            {
                if (signal.Resource == _resource)
                    UpdateState();
            })
            .AddTo(this);
    }

    public void UpdateState()
    {
        var resourceAvailable = PlayerState.Instance.ResourcesAvailable.GetResourceCount(_resource);
        var resourceCapacity = PlayerState.Instance.ResourcesCapacity.GetResourceCount(_resource);
        _label.text = $"<color=white>{resourceAvailable}</color><size=80%>/{resourceCapacity}</size>";
        _slider.value = (float) resourceAvailable / resourceCapacity;
    }
}
