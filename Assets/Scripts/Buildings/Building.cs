using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public abstract class Building : MonoBehaviour, IBuildable
{
    [SerializeField] private string _label;
    [SerializeField] private float _buildDuration;
    [SerializeField] private List<GameResourceAmount> _price;
    
    public string Label => _label;
    public float BuildDuration => _buildDuration;
    public List<GameResourceAmount> Price => _price;
    public float Progress { get; protected set; }

    public async void Build()
    {
        new BuildingEvents.StartedBuild
        {
            Building = this
        }.Publish();
        
        var time = 0f;
        while (time < _buildDuration)
        {
            Progress = time / _buildDuration;
            time += Time.deltaTime;
            await Task.Yield();
        }
        
        new BuildingEvents.FinishedBuilding
        {
            Building = this
        }.Publish();

        StartWork();
    }

    public abstract void StartWork();
}
