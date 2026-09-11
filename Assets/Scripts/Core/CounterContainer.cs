using TMPro;
using UnityEngine;

public class CounterContainer : MonoBehaviour, IPersistentData
{
    public TextMeshProUGUI Text;

    protected CollectableManager CollectableManager;

    private int TotalCoinsCollected;

    private void Awake()
    {
        CollectableManager = FindAnyObjectByType<CollectableManager>();
    }

    public virtual void Start()
    {
        CollectableManager.SubscribeToOnCoinCollected(OnCollected);
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
        CollectableManager.UnsubscribeToOnCoinCollected(OnCollected);
    }
}
