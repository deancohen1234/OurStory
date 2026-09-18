using TMPro;
using UnityEngine;

public class CounterContainer : MonoBehaviour, IPersistentData
{
    protected CollectableManager CollectableManager;

    private void Awake()
    {
        CollectableManager = FindAnyObjectByType<CollectableManager>();
    }

    protected virtual void OnCollected(Pickupable coin)
    {
        
    }

    public virtual void LoadData(GameData data)
    {

    }

    // Update is called once per frame
    public virtual void SaveData(ref GameData data)
    {

    }

    private void OnDestroy()
    {
        CollectableManager.UnsubscribeToCoinCollected(OnCollected);
    }
}
