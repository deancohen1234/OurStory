using UnityEngine;

public class Pickupable : MonoBehaviour, IPersistentData
{
    private AudioSource AudioSource;
    private SpriteRenderer Renderer;

    public void Start()
    {
        AudioSource = GetComponent<AudioSource>();
        Renderer = GetComponentInChildren<SpriteRenderer>();
    }

    void OnTriggerEnter2D()
    {
        OnCollected();
    }

    protected virtual void OnCollected()
    {
        this.AudioSource.Play();

        Renderer.enabled = false;

        Destroy(gameObject, 3);
    }

    void LoadData(GameData data)
    {

    }

    void SaveData(ref GameData data)
    {

    }
}
