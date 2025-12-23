using UnityEngine;

public class LevelLister : MonoBehaviour
{
    public Transform ShipCenter;

    public float SinAmplitude = 10;

    public float SinFreqency = 1;

    Vector2 StartPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPosition = ShipCenter.position;

    }

    // Update is called once per frame
    void Update()
    {
        float listAmount = Mathf.Sin(Time.time * SinFreqency) * SinAmplitude;

        ShipCenter.rotation = Quaternion.Euler(0, 0, listAmount);
        //ShipCenter.position = new Vector2((Time.time * 0.5f) + StartPosition.x, StartPosition.y);
    }
}
