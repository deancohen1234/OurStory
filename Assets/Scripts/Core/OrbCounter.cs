using UnityEngine;

public class OrbCounter : CounterContainer
{
    //these two arrays are 1-1
    public GameObject[] OrbIcons;
    public string[] OrbGuids;

    private SerializeableDictionary<string, bool> OrbsCollectedMap;

    private void Start()
    {
        CollectableManager.SubscribeToOrbCollected(OnOrbCollected);
    }

    private void OnOrbCollected(Orb CollectedOrb)
    {
        string id = CollectedOrb.GetGuid();
        if (OrbsCollectedMap.ContainsKey(id))
        {
            OrbsCollectedMap[id] = true;
        }
        else
        {
            OrbsCollectedMap.Add(id, true);
        }

        UpdateUI();
    }

    public override void LoadData(GameData data)
    {
        FillCollectableDictionary(data);

        //attempt to disable orb icons that have already been found
        UpdateUI();
    }

    // Update is called once per frame
    public override void SaveData(ref GameData data)
    {
        data.OrbObtainedMap = OrbsCollectedMap;
    }

    private void FillCollectableDictionary(GameData data)
    {
        if (OrbsCollectedMap == null)
        {
            OrbsCollectedMap = new SerializeableDictionary<string, bool>();
        }

        foreach (string key in data.OrbObtainedMap.Keys)
        {
            if (!OrbsCollectedMap.ContainsKey(key))
            {
                OrbsCollectedMap.Add(key, data.OrbObtainedMap[key]);
            }
            else
            {
                OrbsCollectedMap[key] = data.OrbObtainedMap[key];
            }
        }
    }

    private void UpdateUI()
    {
        foreach (string key in OrbsCollectedMap.Keys)
        {
            for (int i = 0; i < OrbGuids.Length; i++)
            {
                if (key == OrbGuids[i])
                {
                    bool bIsActive = OrbsCollectedMap[key];

                    CanvasGroup Group = OrbIcons[i].GetComponent<CanvasGroup>();
                    Group.alpha = bIsActive ? 1 : 0;
                }
            }
        }
    }
}
