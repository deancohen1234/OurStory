using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public float m_ShakeFrequency = 1f;
    public float m_ShakeIntensity = 1f;
    public float m_StableTime = 3f;

    private bool m_bIsCrumbling, m_bIsFalling;

    private Vector2 StartLocation;

    private Rigidbody2D m_Rigidbody;

    private float m_FallTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();

        StartLocation = transform.position;
    }

    private void Update()
    {
        if (m_bIsCrumbling)
        {
            if (Time.time > m_FallTime)
            {
                //platform has fallen!
                m_bIsFalling = true;
                m_bIsCrumbling = false;
                m_Rigidbody.bodyType = RigidbodyType2D.Dynamic;
                Debug.Log("Falling For Real");
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!m_bIsCrumbling)
        {
            return;
        }

        float xOffset = Mathf.PerlinNoise(Time.time * m_ShakeFrequency, 0);
        float yOffset = Mathf.PerlinNoise(0, Time.time * m_ShakeFrequency);

        m_Rigidbody.MovePosition(StartLocation + (new Vector2(xOffset, yOffset) * m_ShakeIntensity));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!m_bIsCrumbling && !m_bIsFalling)
        {
            Debug.Log("Starting Fall");
            m_bIsCrumbling = true;

            m_FallTime = Time.time + m_StableTime;
        }
    }
}
