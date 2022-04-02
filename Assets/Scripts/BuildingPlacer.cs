using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BuildingPlacer : MonoBehaviour
{
    public BuildingType DUMMY_TYPE; 

    private GameObject m_ghostBuilding = null;
    private BuildingType m_typeToPlace;
    private bool m_enabled = false;
    private int m_layerMask;

    //---------------------------
    public void Enable(BuildingType i_toPlace)
    {
        m_typeToPlace = i_toPlace;
        m_enabled = true;

        if(m_ghostBuilding != null)
        {
            Destroy(m_ghostBuilding);
        }

        m_ghostBuilding = Instantiate(m_typeToPlace.m_ghost, Vector3.zero, Quaternion.identity);
    }

    //---------------------------
    public void Disable()
    {
        m_enabled = false;
        Destroy(m_ghostBuilding);
    }



    //---------------------------
    void Start()
    {
        m_layerMask = LayerMask.GetMask("PlaceableGround");

        Enable(DUMMY_TYPE);
    }

    //---------------------------
    void Update()
    {
        if(!m_enabled)
        {
            return;
        }

        Vector3 placePos = new Vector3();
        if(GetPlacementPosition(ref placePos))
        {
            //Display ghost building at this position
            m_ghostBuilding.transform.position = placePos;
        }


        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(m_typeToPlace.m_real, placePos, Quaternion.identity);
        }

        //TODO? Rotate buildings
    }


    //---------------------------
    //Raycast from mouse to ground
    bool GetPlacementPosition(ref Vector3 hitPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point);

            hitPos = hit.point;
            return true;
        }

        return false;
    }
}

