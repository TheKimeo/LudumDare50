using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairManager : MonoBehaviour
{
    public FloatReference m_rubbleRepairTime;
    public FloatReference m_buildingRepairRate;
    public FloatReference m_buildingRepairAmount;
	public RectTransform m_RubbleRepairUIPrefab;
    public BuildingType m_buildingType;


    GameObject m_rubble;
    GameObject m_building;
    Health m_health;
    bool m_repairInProg = false;
    float m_repairTimer = 0.0f;

	RectTransform m_RubbleRepairUIInstance;
	UIRadial m_RubbleRepairRadial;

    //listen for dmg

    public bool IsRepairInProgress()
    {
        return m_repairInProg;
    }


    private void OnDestroy()
    {
        if (m_health.HealthRatio > 0)
        {
            foreach (BuildingType.Cost cost in m_buildingType.m_costData)
            {
                //Give back 0.5 build cost when delete
                cost.m_resourceType.Modify(cost.m_buildCost * 0.5f);
            }
        }
    }

    public bool CanAfford()
    {
        if(m_building.activeInHierarchy)
        {
            return true;
        }
        bool canAfford = true;
        //Begin repairing rubble
        foreach (BuildingType.Cost cost in m_buildingType.m_costData)
        {
            canAfford &= cost.m_resourceType.CanConsume(cost.m_buildCost);
        }
        return canAfford;
    }

    public bool CanRepair()
    {
        if(m_building.activeInHierarchy && !m_repairInProg && m_health.HealthRatio < 1.0f)
        {
            return true;
        }
        return CanAfford();

       
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
                    m_repairInProg = true;
                    m_building.GetComponent<BuildingState>().AddSuppressor(this);
                    m_building.GetComponent<HealthVisualiser>().enabled = true;

                }
            }
            else
            {

                Debug.Assert(CanRepair());
                
                foreach (BuildingType.Cost cost in m_buildingType.m_costData)
                {
                    cost.m_resourceType.Modify(-cost.m_buildCost);
                }

                m_repairInProg = true;

                Debug.Assert(m_RubbleRepairUIInstance == null);
                UIPinnedToWorldTransform uiPinManager = UIPinnedToWorldTransform.Instance;
                m_RubbleRepairUIInstance = uiPinManager.InstantiateAndPin(m_RubbleRepairUIPrefab, transform);
                m_RubbleRepairRadial = m_RubbleRepairUIInstance.GetComponentInChildren<UIRadial>();
                
               
              
            }

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
        m_building.GetComponent<HealthVisualiser>().enabled = false;


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
                        m_building.GetComponent<BuildingState>().RemoveSuppressor(this);
                    }
                }

            }
            else
            {
                m_repairTimer += Time.deltaTime;

				if ( m_RubbleRepairRadial != null )
				{
					float ratio = Mathf.Clamp01( m_repairTimer / m_rubbleRepairTime.Value );
					m_RubbleRepairRadial.SetFill( ratio );
				}

                if(m_rubbleRepairTime.Value <= m_repairTimer)
                {
					if ( m_RubbleRepairUIInstance )
					{
						UIPinnedToWorldTransform uiPinManager = UIPinnedToWorldTransform.Instance;
						uiPinManager.DestroyAndRemovePin( m_RubbleRepairUIInstance );
						m_RubbleRepairUIInstance = null;
						m_RubbleRepairRadial = null;
					}

					//Rubble repair complete! 
					m_health.FullHeal();
                    GetComponent<RubbleManager>().RepairBuilding();
                    m_repairTimer = 0;
                    RepairDone();
                }
            }
        }
    }
}
