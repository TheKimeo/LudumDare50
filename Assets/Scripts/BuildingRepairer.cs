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
    public Notification m_noResNotif;
    public NotificationManager m_notifManager;

    public AudioClip m_buildSound;

    AudioSource m_audioSource;

	public BuildingType HoveredBuildingType => m_selectedBuilding?.GetComponent<RepairManager>().m_buildingType;

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
        m_audioSource = GetComponent<AudioSource>();
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
                    if (repairComp.CanRepair())
                    {
                        repairComp.StartRepair();
                        m_audioSource.PlayOneShot(m_buildSound);
                    }
                    else if(!repairComp.CanAfford())
                    {
                        m_notifManager.PushNotif(m_noResNotif);
                    }
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

