using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    public Collectable[] Colllectables;

    public GameObject[] CollectableIcons;

    private int CollectableIndex;

    private OnCoinCollected GlobalDelegate;

    public delegate void OnCollected(Collectable collectable);
    public delegate void OnCoinCollected(Pickupable coin);
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < Colllectables.Length; i++)
        {
            OnCollected onCollectedDelegate = OnCollectableCollected;
            Colllectables[i].AddListener(onCollectedDelegate);
        }
    }

    void OnCollectableCollected(Collectable collectable)
    {
        CanvasGroup Group = CollectableIcons[CollectableIndex].GetComponent<CanvasGroup>();
        Group.alpha = 1;

        CollectableIndex++;

        
    }

    public void OnPickupableCollected(Pickupable coin)
    {
        GlobalDelegate.DynamicInvoke(coin);
    }

    public void SubscribeToOnCoinCollected(OnCoinCollected eventDelegate)
    {
        GlobalDelegate += eventDelegate;
    }

    public void UnsubscribeToOnCoinCollected(OnCoinCollected eventDelegate)
    {
        GlobalDelegate -= eventDelegate;
    }
}
