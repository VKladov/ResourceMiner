﻿using UniRx;

public static class BuildingEvents
{
    public class StartedBuild : Signal
    {
        public Building Building;
    }

    public class FinishedBuilding : Signal
    {
        public Building Building;
    }
    
    public class StartedExtraction : Signal
    {
        public MiningBuilding Building;
    }

    public class FinishedExtraction : Signal
    {
        public MiningBuilding Building;
    }

    public class ExtractedResourcesChanged : Signal
    {
        public MiningBuilding Building;
    }

    public class TakeResourcesRequest : Request
    {
        public MiningBuilding Building;
    }

    public class ShowGridRequest : Request {}

    public class HideGridRequest : Request {}
}