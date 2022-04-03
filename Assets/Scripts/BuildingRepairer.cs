using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingRepairer : Singleton<BuildingRepairer>
{

    public float m_sphereRad = 2;
    private bool m_enabled = false;
    private int m_layerMask;
    GameObject m_selectedBuilding;

    void OnInputModeChange(InputModeManager inputModeManager)
    {
        if (inputModeManager.GetMode() != InputModeManager.Mode.REPAIR)
        {
            m_enabled = false;

            if (m_selectedBuilding != null)
            {
                m_selectedBuilding.GetComponent<ColourSetter>().ResetColour();
                m_selectedBuilding = null;
            }
        }
        else
        {
            m_enabled = true;
        }
    }



    //---------------------------
    void Start()
    {
        m_layerMask = LayerMask.GetMask("Building");

        InputModeManager.Instance.m_onModeChange.AddListener(OnInputModeChange);
    }

    //---------------------------
    void Update()
    {
        if (!m_enabled)
        {
            return;
        }

        GameObject selected = GetSelectedBuilding();
        if (selected != null)
        {
            RepairManager repairComp = selected.GetComponent<RepairManager>();
            Debug.Assert(repairComp != null);
            bool repairInProgress = repairComp.IsRepairInProgress();
            if (!repairInProgress)
            {
                selected.GetComponent<ColourSetter>().SetColour(Color.cyan);
            }

            if (selected != m_selectedBuilding && m_selectedBuilding != null)
            {
                m_selectedBuilding.GetComponent<ColourSetter>().ResetColour();
            }
            m_selectedBuilding = selected;

            bool mouseDown = Input.GetMouseButtonDown(0);
            bool mouseOverUI = EventSystem.current.IsPointerOverGameObject();
            if (mouseDown && mouseOverUI == false)
            {
                if (!repairInProgress)
                {
                    repairComp.StartRepair();
                    selected.transform.Find("Building").gameObject.GetComponent<HealthVisualiser>().enabled = true;
                }
                m_selectedBuilding.GetComponent<ColourSetter>().ResetColour();
                m_selectedBuilding = null;
            }
        }
        else if (m_selectedBuilding != null)
        {
            m_selectedBuilding.GetComponent<ColourSetter>().ResetColour();
            m_selectedBuilding = null;
        }

        bool exitMode = Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape);
        if (exitMode)
        {
            InputModeManager.Instance.SetMode(InputModeManager.Mode.NONE);
            return;
        }
    }


    //---------------------------
    //Raycast from mouse to ground
    GameObject GetSelectedBuilding()
    {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit hit;
        if (Physics.SphereCast(point, m_sphereRad, Camera.main.transform.forward, out hit, Mathf.Infinity, m_layerMask, QueryTriggerInteraction.Ignore))
        {

            return hit.collider.gameObject.transform.root.gameObject;

        }

        return null;
    }
}

