using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int TotalCoins;

    public int TotalCollectablesObtained;

    public Dictionary<int, bool> CollectableObtainedMap;

    public GameData()
    {
        TotalCoins = 0;
        TotalCollectablesObtained = 0;
    }

    public bool IsCollected(int index)
    {
        return CollectableObtainedMap[index];
    }
}
