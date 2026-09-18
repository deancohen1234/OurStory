using UnityEngine;

public class Pickupable : MonoBehaviour
{
    private AudioSource AudioSource;
    private SpriteRenderer Renderer;
    private Rigidbody2D Rigidbody;

    protected CollectableManager CollectableManager;

    private bool bIsInitialized = false;
    private bool bMarkedForDestroy = false;

    public virtual void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
        Rigidbody = GetComponent<Rigidbody2D>();

        CollectableManager = FindAnyObjectByType<CollectableManager>();

        bIsInitialized = true;

        if (bMarkedForDestroy)
        {
            DestroyPickupable();
        }
    }

    void OnTriggerEnter2D()
    {
        OnCollected();
    }

    protected virtual void OnCollected()
    {
        this.AudioSource.Play();

        DestroyPickupable();
    }

    protected virtual void DestroyPickupable()
    {
        if (bIsInitialized == false)
        {
            bMarkedForDestroy = true;
            return;
        }
        Renderer.enabled = false;
        Rigidbody.simulated = false;
        bMarkedForDestroy = true;

        Destroy(gameObject, 3);
    }
}
