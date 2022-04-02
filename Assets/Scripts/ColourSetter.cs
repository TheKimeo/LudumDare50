using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSetter : MonoBehaviour
{
    List<Material> m_materials;
    List<Color> m_baseColours;

    // Start is called before the first frame update
    void Awake()
    {
        m_materials = new List<Material>();
        m_baseColours = new List<Color>();
        //Get material data for all child objects and self. There will be repeats, shouldnt matter

        m_materials.Add(GetComponent<Renderer>().material);
        m_baseColours.Add(GetComponent<Renderer>().material.GetColor("_Color"));

        foreach (Transform child in transform)
        {
            Renderer rend = transform.gameObject.GetComponent<Renderer>();
            if(rend != null)
            {
                m_materials.Add(rend.material);
                m_baseColours.Add(rend.material.GetColor("_Color"));
            }
        }
    }

    public void SetColour(Color i_colour)
    {
        foreach (Material mat in m_materials)
        {
            mat.SetColor("_Color", i_colour);
        }
    }

    public void ResetColour()
    {
        for (int i = 0; i < m_materials.Count; ++i)
        {
            m_materials[i].SetColor("_Color", m_baseColours[i]);
        }
    }
}
