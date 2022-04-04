using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound3D : MonoBehaviour
{
    public float m_minMult = 0.01f;
    AudioSource m_audioSource;
    float m_baseVol;
    CameraControl m_camScript;
    // Start is called before the first frame update
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_baseVol = m_audioSource.volume;
        m_camScript = Camera.main.GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        m_audioSource.volume = m_baseVol * Mathf.Max(m_camScript.GetZoomRatio(), m_minMult);
    }
}
