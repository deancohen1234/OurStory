using UnityEngine;

public class Collectable : MonoBehaviour
{
    private CollectableManager.OnCollected Delegate;

    public void AddListener(CollectableManager.OnCollected Delegate)
    {
        this.Delegate = Delegate;
    }

    void OnTriggerEnter2D()
    {
        OnCollected();
    }

    void OnCollected()
    {
        Delegate.DynamicInvoke(this);

        Destroy(gameObject);
    }
}
