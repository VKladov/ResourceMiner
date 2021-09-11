using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildable
{
    public string Label { get; }
    public float BuildDuration { get; }
    public List<GameResourceAmount> Price { get; }
}
