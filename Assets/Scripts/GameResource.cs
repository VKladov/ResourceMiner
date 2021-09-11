using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "ScriptableObjects/Resource", order = 1)]
public class GameResource : CustomScriptableObject<GameResource>
{
    [SerializeField] private Sprite _icon;

    public Sprite Icon => _icon;

    public static GameResource Mineral => GetByName("Mineral");
    public static GameResource Gas => GetByName("Gas");
}
