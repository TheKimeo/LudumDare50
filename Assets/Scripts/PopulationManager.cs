using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public Population m_population;

    // Start is called before the first frame update
    void Start()
    {
        m_population.Initialise();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
