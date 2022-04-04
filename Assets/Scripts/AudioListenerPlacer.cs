using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListenerPlacer : MonoBehaviour
{

    public GameObject m_audioListener;
    private int m_layerMask;

    // Start is called before the first frame update
    void Start()
    {
        m_layerMask = LayerMask.GetMask("PlaceableGround");

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit,Mathf.Infinity, m_layerMask))
        {
            m_audioListener.transform.position = hit.point;
        }
    }
}
