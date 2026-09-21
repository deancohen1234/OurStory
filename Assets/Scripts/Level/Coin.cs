using UnityEngine;

public class Coin : Pickupable
{
    public float Amplitude = 0.2f;
    public float Frequency = 1.0f;
    public float XMultiplier = 1.0f;

    public ParticleSystem CollectedSystem;

    private float StartingYValue = 0;

    public override void Start()
    {
        base.Start();

        StartingYValue = transform.localPosition.y;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin((Time.time + (transform.localPosition.x * XMultiplier)) * Frequency) * Amplitude;
        Vector2 newPosition = new Vector2 (transform.localPosition.x, StartingYValue + yOffset);

        transform.localPosition = newPosition;
    }

    protected override void OnCollected()
    {
        base.OnCollected();

        this.CollectableManager.OnCoinCollected(this);
        CollectedSystem.Play();
    }
}
