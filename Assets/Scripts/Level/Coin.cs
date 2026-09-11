using UnityEngine;

public class Coin : Pickupable
{
    public float Amplitude = 0.2f;
    public float Frequency = 1.0f;
    public float XMultiplier = 1.0f;

    private float StartingYValue = 0;

    private CollectableManager CollectableManager;

    public override void Start()
    {
        base.Start();
        this.CollectableManager = FindAnyObjectByType<CollectableManager>();

        StartingYValue = transform.position.y;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin((Time.time + (transform.position.x * XMultiplier)) * Frequency) * Amplitude;
        Vector2 newPosition = new Vector2 (transform.position.x, StartingYValue + yOffset);

        transform.position = newPosition;
    }

    protected override void OnCollected()
    {
        base.OnCollected();
        this.CollectableManager.OnPickupableCollected(this);
    }
}
