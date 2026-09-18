using UnityEngine;
using UnityEngine.UI;

public class CollectableManager : MonoBehaviour
{
    public Orb[] Colllectables;

    private int CollectableIndex;

    private OnOrbCollectedEvent OnOrbCollectedDelegate;
    private OnCoinCollectedEvent OnCoinCollectedDelegate;

    public delegate void OnOrbCollectedEvent(Orb collectable);
    public delegate void OnCoinCollectedEvent(Coin coin);

    public void OnCoinCollected(Coin coin)
    {
        OnCoinCollectedDelegate.DynamicInvoke(coin);
    }

    public void OnOrbCollected(Orb orb)
    {
        OnOrbCollectedDelegate.DynamicInvoke(orb);
    }

    public void SubscribeToOrbCollected(OnOrbCollectedEvent eventDelegate)
    {
        OnOrbCollectedDelegate += eventDelegate;
    }

    public void UnsubscribeToOrbCollected(OnOrbCollectedEvent eventDelegate)
    {
        OnOrbCollectedDelegate -= eventDelegate;
    }

    public void SubscribeToCoinCollected(OnCoinCollectedEvent eventDelegate)
    {
        OnCoinCollectedDelegate += eventDelegate;
    }

    public void UnsubscribeToCoinCollected(OnCoinCollectedEvent eventDelegate)
    {
        OnCoinCollectedDelegate -= eventDelegate;
    }
}
