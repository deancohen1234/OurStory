using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int TotalCoins;

    public int TotalCollectablesObtained;

    public Dictionary<int, bool> CollectableObtainedMap;

    public bool IsCollected(int index)
    {
        return CollectableObtainedMap[index];
    }
}
