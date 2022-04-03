using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationModifer : MonoBehaviour
{
    public Population m_population;
    public int m_popCapMod = 100;
    void OnDisable()
    {
        m_population.ModifyCap(-m_popCapMod);
    }

    void OnEnable()
    {
        m_population.ModifyCap(m_popCapMod);
    }
}
