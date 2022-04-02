using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFoundation : MonoBehaviour
{
    private int m_layerMask;
    private bool m_safeToPlace = true;
    private ColourSetter m_colComp;


    public bool IsSafeToPlace()
    {
        return m_safeToPlace;
    }

    void Start()
    {
        m_layerMask = LayerMask.GetMask("Building", "Crater");
        m_colComp = GetComponent<ColourSetter>();
        m_colComp.SetColour(Color.green);
    }

    void OnTriggerEnter(Collider other)
    {
        if ((m_layerMask & (1 << other.gameObject.layer)) != 0)
        {
            m_safeToPlace = false;
            m_colComp.SetColour(Color.red);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if ((m_layerMask & (1 << other.gameObject.layer)) != 0)
        {
            m_safeToPlace = true;
            m_colComp.SetColour(Color.green);

        }
    }

}
