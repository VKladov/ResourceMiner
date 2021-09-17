using System;
using System.Linq;
using UnityEngine;

public class CustomScriptableObject<T> : ScriptableObject where T : UnityEngine.Object
{
    private static T[] _all;

    public static T[] All
    {
        get
        {
            if (_all == null)
                _all = Resources.LoadAll<T>(string.Empty);

            return _all;
        }
    }

    public static T GetByName(string name)
    {
        var item = All.FirstOrDefault(item => item.name == name);
        if (item == null)
            throw new Exception("Could not find " + typeof(T) + " with name " + name);

        return item;
    }

    public static bool TryGetByName(string name, out T result)
    {
        result = All.FirstOrDefault(item => item.name == name);
        return result != null;
    }
}