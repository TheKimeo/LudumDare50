using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSoundEmitter : MonoBehaviour
{

    public AudioClip m_dmgSound;
    AudioSource m_audioSource;
    public Health m_health;
    float m_lastHealthVal = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();

        m_health.m_OnDamageEvent.AddListener(OnDamaged);
   

    }

    private void OnDamaged(Health health)
    {
       if(!m_audioSource.isPlaying)
        {
            if (health.HealthRatio < m_lastHealthVal)
            {
                m_audioSource.PlayOneShot(m_dmgSound);
            }

        }
        m_lastHealthVal = health.HealthRatio;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
