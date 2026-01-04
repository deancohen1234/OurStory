using UnityEngine;
using System.Collections.Generic;

public class WindArea : MonoBehaviour
{
    public float WindForce = 20f;

    private List<Rigidbody2D> m_Rigidbodies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbodies = new List<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (m_Rigidbodies != null && m_Rigidbodies.Count > 0)
        {
            for (int i = 0; i < m_Rigidbodies.Count; i++)
            {
                m_Rigidbodies[i].AddForce(transform.right * WindForce * Time.fixedDeltaTime);
            }
        }     
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_Rigidbodies.Add(collision.attachedRigidbody);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_Rigidbodies.Remove(collision.attachedRigidbody);
    }
}
