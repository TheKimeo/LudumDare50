using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ColourSetter : MonoBehaviour
{
    List<Material> m_materials;
    List<Color> m_baseColours;



    void PopulateChildMats(Transform i_parent)
    {    
        foreach (Transform child in i_parent)
        {
            //Recursion baybeee
            PopulateChildMats(child);

            Renderer rend = child.gameObject.GetComponent<Renderer>();
            if (rend != null)
            {
                List<Material> mats = rend.materials.ToList();
                foreach(Material mat in mats )
                {
                    m_materials.Add(mat);
                    m_baseColours.Add(mat.GetColor("_Color"));
                }
            }
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_materials = new List<Material>();
        m_baseColours = new List<Color>();

        //Get material data for all child objects and self. There will be repeats, shouldnt matter
        {
            Renderer rend = GetComponent<Renderer>();

            if (rend != null)
            {
                List<Material> mats = rend.materials.ToList();
                foreach (Material mat in mats)
                {
                    m_materials.Add(mat);
                    m_baseColours.Add(mat.GetColor("_Color"));
                }

          
            }
        }
        PopulateChildMats(transform);

    }

    public void SetColour(Color i_colour)
    {
        if (m_materials != null)
        {
            foreach (Material mat in m_materials)
            {
                mat.SetColor("_Color", i_colour);
            }
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
