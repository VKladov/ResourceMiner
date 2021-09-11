using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public static class UIEvents
{
    public class BuildingSelectButtonPressed : Signal
    {
        public Building Building;
    }
    
    public class PlaceBuilding : Request
    {
        public Building Building;
        public Vector2Int Cell;
    }

    public class BuildingPlaced : Signal
    {
        public Building Building;
    }
    
    public class TakeBuildingResources : Request
    {
        public Building Building;
    }
}
