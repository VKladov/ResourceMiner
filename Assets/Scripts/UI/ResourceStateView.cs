using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ResourceStateView : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private ResourceAmountView _amountView;

    private GameResource _resource;
    public void Init(GameResource resource)
    {
        _resource = resource;
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
        _amountView.Show(_resource.Icon, resourceAvailable);
        _slider.value = (float) resourceAvailable / resourceCapacity;
    }
}
