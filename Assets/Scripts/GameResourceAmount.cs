using System;

[Serializable]
public class GameResourceAmount
{
    public GameResource Resource;
    public int Amount;

    public GameResourceAmount Copy()
    {
        return new GameResourceAmount
        {
            Resource = Resource,
            Amount = Amount
        };
    }
}
