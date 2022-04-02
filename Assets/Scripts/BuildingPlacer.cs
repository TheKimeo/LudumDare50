using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BuildingPlacer : MonoBehaviour
{
    public BuildingPrefabFactory m_buildingPrefabFactory;

    private GameObject m_ghostBuilding;
    private BuildingTypes m_typeToPlace = BuildingTypes.INVALID;
    private bool m_enabled = false;
    private int m_layerMask;

    //---------------------------
    public void Enable(BuildingTypes i_toPlace)
    {
        m_typeToPlace = i_toPlace;
        m_enabled = true;

        m_ghostBuilding = m_buildingPrefabFactory.CreateGhost(m_typeToPlace, Vector3.zero);
    }

    //---------------------------
    public void Disable()
    {
        m_typeToPlace = BuildingTypes.INVALID;
        m_enabled = false;
    }

    //---------------------------
    void Start()
    {
        m_layerMask = LayerMask.GetMask("PlaceableGround");

        Enable(BuildingTypes.Test1);
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
            m_buildingPrefabFactory.CreateReal(m_typeToPlace, placePos);
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

