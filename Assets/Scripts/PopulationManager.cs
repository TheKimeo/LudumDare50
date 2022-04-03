using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationManager : MonoBehaviour
{
    public Population m_population;
    public Notification m_popFoodAlert;
    public float m_alertRateLimit = 10.0f;
    public NotificationManager m_notifManager;

    float m_tSinceAlert = 0.0f;
    float m_tSinceFoodConsumption = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_population.Initialise();
        m_tSinceAlert = m_alertRateLimit;
    }

    // Update is called once per frame
    void Update()
    {
        m_tSinceAlert += Time.deltaTime;
        m_tSinceFoodConsumption += Time.deltaTime;

        if (m_tSinceFoodConsumption >= 1.0f)
        {
            float toConsume = -m_population.m_Value / m_population.m_cap * m_population.m_foodConsumptionRate;
            if (!m_population.m_foodResource.CanConsume(-toConsume) && m_tSinceAlert >= m_alertRateLimit)
            {
                m_notifManager.PushNotif(m_popFoodAlert);
                m_tSinceAlert = 0.0f;
            }

            m_population.m_foodResource.Modify(toConsume);

            m_tSinceFoodConsumption = 0f;
        }
    }
}
