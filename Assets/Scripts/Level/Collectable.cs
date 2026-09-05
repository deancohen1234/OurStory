using UnityEngine;

public class Collectable : Pickupable, IPersistentData
{
    private CollectableManager.OnCollected Delegate;
    private AudioSource AudioSource;
    private SpriteRenderer Renderer;


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
