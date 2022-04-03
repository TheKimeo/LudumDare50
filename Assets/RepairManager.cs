using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairManager : MonoBehaviour
{
    public FloatReference m_rubbleRepairTime;
    public FloatReference m_buildingRepairRate;
    public FloatReference m_buildingRepairAmount;

    GameObject m_rubble;
    GameObject m_building;
    Health m_health;
    bool m_repairInProg = false;
    float m_repairTimer = 0.0f;


    //listen for dmg

    public bool IsRepairInProgress()
    {
        return m_repairInProg;
    }

    public void StartRepair()
    {
        //Ignore redundant req
        if(!m_repairInProg)
        {
            m_repairTimer = 0.0f;

            if (m_building.activeInHierarchy)
            {
                if (m_health.HealthRatio < 1.0f)
                {
                    //Begin repairing building
                }
            }
            else
            {
                //Begin repairing rubble
            }

            m_repairInProg = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_building = transform.Find("Building").gameObject;
        Debug.Assert(m_building != null, "Couldnt find building child obj!");
        m_rubble = transform.Find("Rubble").gameObject;
        Debug.Assert(m_rubble != null, "Couldnt find rubble child obj!");

        m_health = m_building.GetComponent<Health>();
    }


    void RepairDone()
    {
        //TODO ui
        m_repairInProg = false;


    }

    // Update is called once per frame
    void Update()
    {
        if(m_repairInProg)
        {
            if(m_building.activeInHierarchy)
            {
                m_repairTimer += Time.deltaTime;
                if(m_repairTimer >= m_buildingRepairRate.Value)
                {
                    m_health.Modify(m_buildingRepairAmount.Value);
                    m_repairTimer = 0.0f;

                    if(m_health.HealthRatio >= 1)
                    {
                        RepairDone();
                    }
                }

            }
            else
            {
                m_repairTimer += Time.deltaTime;

                if(m_rubbleRepairTime.Value <= m_repairTimer)
                {
                    //Rubble repair complete! 
                    GetComponent<RubbleManager>().RepairBuilding();
                    m_health.FullHeal();
                    m_repairTimer = 0;
                    RepairDone();
                }
            }
        }
    }
}
