using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MiningBuilding : Building
{
    [SerializeField] private GameResourceAmount _extractingResource;
    [SerializeField] private float _extractDuration = 5;

    public GameResourceAmount ExtractingResource => _extractingResource;
    public GameResourceAmount ExtractedResource { get; private set; }
    public bool Full { get; private set; }

    public void TakeResource(int max, out int amountTaken)
    {
        if (ExtractedResource == null)
        {
            amountTaken = 0;
            return;
        }
        
        amountTaken = Mathf.Min(max, ExtractedResource.Amount);
        ExtractedResource.Amount -= amountTaken;

        if (ExtractedResource.Amount == 0)
            Full = false;
        
        new BuildingEvents.ExtractedResourcesChanged()
        {
            Building = this
        }.Publish();
    }
    public override async void StartWork()
    {
        while (enabled)
        {
            if (Full)
                await Task.Yield();
            else
                await ExtractResources();
        }
    }

    public override string GetDescription()
    {
        return $"Produces {_extractingResource.Amount} units of {_extractingResource.Resource.name} for {_extractDuration} seconds";
    }

    private async Task ExtractResources()
    {
        new BuildingEvents.StartedExtraction
        {
            Building = this
        }.Publish();
        
        var time = 0f;
        while (time < _extractDuration)
        {
            Progress = time / _extractDuration;
            time += Time.deltaTime;
            await Task.Yield();
        }

        ExtractedResource = _extractingResource.Copy();
        Full = true;
        
        new BuildingEvents.FinishedExtraction
        {
            Building = this
        }.Publish();
        
        new BuildingEvents.ExtractedResourcesChanged()
        {
            Building = this
        }.Publish();
    }
}
