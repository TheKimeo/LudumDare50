using UnityEngine;

public class PopulationManager : MonoBehaviour
{
  [SerializeField] Population m_population;
	[SerializeField] float m_TimeUnderCapForRequest = 10.0f;
	[SerializeField] float m_RequestDelayToStart = 30.0f;
	[SerializeField] float m_DelayAfterRequest = 10.0f;
	[SerializeField] EventBehaviour m_RocketEvent;
    [SerializeField]  Notification m_popFoodAlert;
    [SerializeField]  float m_alertRateLimit = 10.0f;
    [SerializeField]  NotificationManager m_notifManager;
    
	float m_CheckNextRequestTime;
	float m_TimeUntilRequest;
  
    float m_tSinceAlert = 0.0f;
    float m_tSinceFoodConsumption = 0.0f;

    void Start()
    {
		TimeManager timeManager = TimeManager.Instance;

        m_population.Initialise();
        m_tSinceAlert = m_alertRateLimit;
    }

		m_CheckNextRequestTime = timeManager.m_CurrentTime;
		m_TimeUntilRequest = m_TimeUnderCapForRequest;
	}

    void Update()
    {
      UpdateRocketRequest();
      UpdateFoodConsumpsion();
	}
  
  void UpdateRocketRequest()
  {
    TimeManager timeManager = TimeManager.Instance;
		float currentTime = timeManager.m_CurrentTime;

		if ( m_CheckNextRequestTime > currentTime )
		{
			//Waiting until enough time has passed after our previous request
			m_TimeUntilRequest = m_TimeUnderCapForRequest;
			return;
		}

		if (m_population.m_Value >= m_population.m_cap )
		{
			m_TimeUntilRequest = m_TimeUnderCapForRequest;
		}
		else
		{
			m_TimeUntilRequest -= Time.deltaTime;
		}

		if ( m_TimeUntilRequest > 0.0f )
		{
			//Not enough time has passed under the population cap
			return;
		}

		EventManager.Event rocketEvent = EventManager.Instance.QueueEvent( m_RocketEvent, m_RequestDelayToStart );
		m_CheckNextRequestTime = currentTime + rocketEvent.m_StartTime + rocketEvent.m_Duration + m_DelayAfterRequest;
		m_TimeUntilRequest = m_TimeUnderCapForRequest;
  }

void UpdateFoodConsumpsion()
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
