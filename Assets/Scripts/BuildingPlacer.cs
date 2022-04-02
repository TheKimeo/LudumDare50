using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : Singleton<BuildingPlacer>
{
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

		bool exitMode = Input.GetMouseButtonDown( 1 ) || Input.GetKeyDown(KeyCode.Escape);
		if ( exitMode )
		{
			Disable();
			return;
		}

		bool mouseDown = Input.GetMouseButtonDown( 0 );
		bool mouseOverUI = EventSystem.current.IsPointerOverGameObject();
        if ( mouseDown && mouseOverUI == false )
        {
            if (m_ghostBuilding.GetComponent<BuildingFoundation>().IsSafeToPlace())
            {
                Instantiate(m_typeToPlace.m_real, placePos, Quaternion.identity);
            }
            else
            {
                //TODO fire off a msg saying nope
            }
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

