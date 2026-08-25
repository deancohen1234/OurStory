using UnityEngine;

public class Collectable : MonoBehaviour
{
    private CollectableManager.OnCollected Delegate;
    private AudioSource AudioSource;
    private SpriteRenderer Renderer;

    public void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
    }

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
        this.AudioSource.Play();

        Delegate.DynamicInvoke(this);

        Renderer.enabled = false;
        Destroy(gameObject, 3);
    }
}
