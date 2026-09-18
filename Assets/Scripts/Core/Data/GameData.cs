using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int TotalCoins;

    public int TotalCollectablesObtained;

    public SerializeableDictionary<string, bool> OrbObtainedMap;

    public GameData()
    {
        TotalCoins = 0;
        TotalCollectablesObtained = 0;
        OrbObtainedMap = new SerializeableDictionary<string, bool>();
    }

    public bool IsCollected(string id)
    {
        return OrbObtainedMap[id];
    }
}
