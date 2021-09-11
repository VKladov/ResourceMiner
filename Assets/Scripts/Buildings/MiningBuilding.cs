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
    public bool Full { get; private set; }

    public void Clear()
    {
        Full = false;
        new BuildingEvents.BecameEmpty
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

        Full = true;
        
        new BuildingEvents.FinishedExtraction
        {
            Building = this
        }.Publish();
    }
}
