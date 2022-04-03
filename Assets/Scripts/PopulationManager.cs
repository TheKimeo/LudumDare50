using UnityEngine;

public class PopulationManager : MonoBehaviour
{
	[SerializeField] Population m_population;
	[SerializeField] float m_TimeUnderCapForRequest = 10.0f;
	[SerializeField] float m_RequestDelayToStart = 30.0f;
	[SerializeField] float m_DelayAfterRequest = 10.0f;
	[SerializeField] EventBehaviour m_RocketEvent;
	[SerializeField] Notification m_popFoodAlert;
	[SerializeField] NotificationManager m_notifManager;
	[SerializeField] float m_famineRate = 1.0f;


	float m_CheckNextRequestTime;
	float m_TimeUntilRequest;

	void Start()
	{
		TimeManager timeManager = TimeManager.Instance;

		m_population.Initialise();

		m_CheckNextRequestTime = timeManager.m_CurrentTime;
		m_TimeUntilRequest = m_TimeUnderCapForRequest;
	}

	private void OnEnable()
	{
		ResourceManager resourceManager = ResourceManager.Instance;
		resourceManager.m_OnTickEvent += OnResourceTick;
	}

	private void OnDisable()
	{
		ResourceManager resourceManager = ResourceManager.Instance;
		resourceManager.m_OnTickEvent -= OnResourceTick;
	}

	void Update()
	{
		UpdateRocketRequest();
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

		if ( m_population.m_Value >= m_population.m_cap )
		{
			m_TimeUntilRequest = m_TimeUnderCapForRequest;
		}
		else if ( RocketLandingMarker.AllLandingMarkers.Count > 0 )
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

	void OnResourceTick()
	{

		float toConsume = -m_population.m_Value * m_population.m_foodConsumptionRate;
		if ( m_population.m_foodResource.CanConsume( -toConsume ) == false )
		{
			m_notifManager.PushNotif( m_popFoodAlert );
			m_population.Modify(-m_famineRate);
		}

		m_population.m_foodResource.Modify( toConsume );
	}
}
