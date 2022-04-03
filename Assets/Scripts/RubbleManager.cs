using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RubbleManager : MonoBehaviour
{
    public class OnDestroy : UnityEvent<RubbleManager> { }
    public class OnRepair : UnityEvent<RubbleManager> { }

    [SerializeField] public OnDestroy m_onDestroy = new OnDestroy();
    [SerializeField] public OnRepair m_onRepair = new OnRepair();
    [SerializeField] GameObject m_destroyEffect;

    GameObject m_rubble;
    GameObject m_building;
    Health m_Health;

    // Start is called before the first frame update
    void Start()
    {
        m_rubble = transform.Find("Rubble").gameObject;
        Debug.Assert(m_rubble != null, "Couldnt find rubble child obj!");
        m_building = transform.Find("Building").gameObject;
        Debug.Assert(m_rubble != null, "Couldnt find building child obj!");
        m_Health = m_building.GetComponent<Health>();
        m_Health.m_OnDamageEvent.AddListener(OnDamaged);
        m_rubble.GetComponent<RandomVertexDamage>().SetDamage(1);
        m_rubble.SetActive(false);
    }

    private void OnDamaged(Health health)
    {
        if(health.HealthRatio == 0)
        {
            DestroyBuilding();
        }
    }

    void DestroyBuilding()
    {
        m_onDestroy?.Invoke(this);

        GameObject.Instantiate(m_destroyEffect, transform);


        m_building.SetActive(false);
        m_rubble.SetActive(true);
    }

    public void RepairBuilding()
    {
        m_onRepair?.Invoke(this);

        m_building.SetActive(true);
        m_rubble.SetActive(false);
    }



}
