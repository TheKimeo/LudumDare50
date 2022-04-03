using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingPlacer : Singleton<BuildingPlacer>
{
    public NotificationManager m_notifMngr;
    public Notification m_invalidPosNotif;
    public BuildingType m_defaultType;


    private GameObject m_ghostBuilding = null;
    private BuildingType m_typeToPlace = null;
    private bool m_enabled = false;
    private int m_layerMask;

    //---------------------------
    public void SetType(BuildingType i_toPlace)
    {
        m_typeToPlace = i_toPlace;

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

    void OnInputModeChange(InputModeManager inputModeManager)
    {
        if(inputModeManager.GetMode() != InputModeManager.Mode.BUILD)
        {
            Disable();
        }
        else
        {
            m_enabled = true;
        }
    }

    //---------------------------
    void Start()
    {
        m_layerMask = LayerMask.GetMask("PlaceableGround");

        InputModeManager.Instance.m_onModeChange.AddListener(OnInputModeChange);


    }

    //---------------------------
    void Update()
    {
        if(!m_enabled || InputModeManager.Instance.GetMode() != InputModeManager.Mode.BUILD || m_typeToPlace == null)
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
            InputModeManager.Instance.SetMode(InputModeManager.Mode.NONE);
			return;
		}

		bool mouseDown = Input.GetMouseButtonDown( 0 );
		bool mouseOverUI = EventSystem.current.IsPointerOverGameObject();
        if ( mouseDown && mouseOverUI == false )
        {
            if (m_ghostBuilding.GetComponent<BuildingFoundation>().IsSafeToPlace())
            {
                bool canBuild = true;
                foreach(BuildingType.Cost cost in m_typeToPlace.m_costData)
                {
                    if(!cost.m_resourceType.CanConsume(cost.m_buildCost))
                    {
                        //Todo fire off a message for each type that we dont have enough of
                        canBuild = false;
                        
                        //Trigger callback with our notification
                        
                        m_notifMngr.PushNotif(cost.m_resourceType.m_insufficientNotif);
                        
                    }
               
                }

                if (canBuild)
                {
                    foreach (BuildingType.Cost cost in m_typeToPlace.m_costData)
                    {
                        cost.m_resourceType.Modify(-cost.m_buildCost);
                    }
                    Instantiate(m_typeToPlace.m_real, placePos, Quaternion.identity);

                }
            }
            else
            {
               
                m_notifMngr.PushNotif(m_invalidPosNotif);
                
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

