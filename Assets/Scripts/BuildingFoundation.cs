using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFoundation : MonoBehaviour
{
    public BuildingType m_type;

    private int m_layerMask;
    private bool m_safeToPlace = true;
    private ColourSetter m_colComp;
    private uint m_colCounter = 0;
    bool m_isSnapped = false;
    bool m_snapMode = false;

      
    bool CanAfford()
    {
        bool canAfford = true; 
        foreach (BuildingType.Cost cost in m_type.m_costData)
        {
            canAfford &= cost.m_resourceType.CanConsume(cost.m_buildCost);
        }
        return canAfford;
    }
    public void SetSnapped(bool i_val)
    {
        Debug.Assert(m_snapMode);
        m_isSnapped = i_val;

        if(m_isSnapped)
        {
            m_colComp.SetColour(Color.green);
        }
        else
        {
            m_colComp.SetColour(Color.red);
        }
    }

    public bool IsSafeToPlace()
    {
        if (m_snapMode)
        {
            return m_isSnapped && CanAfford() ;
        }
        else
        {
            return m_safeToPlace && CanAfford();
        }
    }

    void Awake()
    {
        m_layerMask = LayerMask.GetMask("Building", "Crater");
        m_colComp = GetComponent<ColourSetter>();

        m_snapMode = m_type.m_snapLayer != "";
        if(m_snapMode)
        {
            m_colComp.SetColour(Color.red);

        }
        else
        {
            m_colComp.SetColour(Color.green);

        }
    }

    void Update()
    {
        Debug.Log("hhhh");
        bool canAfford = CanAfford();
        if (canAfford && m_colCounter == 0)
        {
            m_colComp.SetColour(Color.green);
        }
        else if(!canAfford)
        {
            m_colComp.SetColour(Color.red);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!m_snapMode &&(m_layerMask & (1 << other.gameObject.layer)) != 0)
        {
            m_safeToPlace = false;
            m_colComp.SetColour(Color.red);
            ++m_colCounter;

        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!m_snapMode && (m_layerMask & (1 << other.gameObject.layer)) != 0 && m_colCounter > 0)
        {
            --m_colCounter;
            if (m_colCounter == 0)
            {
                m_safeToPlace = true;
                m_colComp.SetColour(Color.green);

            }
        }
    }

}
