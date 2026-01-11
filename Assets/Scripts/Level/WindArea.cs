using UnityEngine;
using System.Collections.Generic;

public class WindArea : MonoBehaviour
{
    public float WindForce = 20f;
    public ParticleSystem System;

    public float ParticleSystemSpeedMin = 4f;
    public float ParticleSystemSpeedMax = 20f;

    public float WindForceMin = 10f;
    public float WindForceMax = 100f;


    private List<Rigidbody2D> m_Rigidbodies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Rigidbodies = new List<Rigidbody2D>();     
    }

    private void Update()
    {
        if (System)
        {
            ParticleSystem.VelocityOverLifetimeModule module = System.velocityOverLifetime;

            float normalizedSpeed = Mathf.Clamp(WindForce - WindForceMin, 0, (WindForceMax - WindForceMin)) / (WindForceMax - WindForceMin);

            float multiplier = ((ParticleSystemSpeedMax - ParticleSystemSpeedMin) * normalizedSpeed) + ParticleSystemSpeedMin;

            module.speedModifierMultiplier = multiplier;
        }
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
