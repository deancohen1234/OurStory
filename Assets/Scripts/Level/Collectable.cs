using UnityEngine;

public class Collectable : Pickupable
{
    private CollectableManager.OnCollected Delegate;

    public void AddListener(CollectableManager.OnCollected Delegate)
    {
        this.Delegate = Delegate;
    }

    protected override void OnCollected()
    {
        base.OnCollected();

        Delegate.DynamicInvoke(this);
    }

    void LoadData(GameData data)
    {

    }

    void SaveData(ref GameData data)
    {

    }
}
