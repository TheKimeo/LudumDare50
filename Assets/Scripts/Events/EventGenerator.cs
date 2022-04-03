using UnityEngine;

public class EventGenerator : MonoBehaviour
{
	[SerializeField] EventBehaviour m_ToGenerate;
	[SerializeField] float m_InitialDelayBetweenEvents = 15.0f;
	[SerializeField] float m_EventGenerationOffset = 30.0f; //May need to scale with how far the player can see ahead
	
	EventManager m_EventManager;
	TimeManager m_TimeManager;
	DifficultyManager m_DifficultyManager;

	float m_NextEventTime;

	void Start()
	{
		Debug.Assert( m_InitialDelayBetweenEvents > 0.5f, "[EventGenerator] Will generate too many events" );

		m_EventManager = EventManager.Instance;
		m_TimeManager = TimeManager.Instance;
		m_DifficultyManager = DifficultyManager.Instance;

		m_NextEventTime = 0.0f;
	}

	private void Update()
	{
		while ( m_NextEventTime < m_TimeManager.m_CurrentTime )
		{
			QueueEvent();
			m_NextEventTime += CalculateEventDelay();
		}
	}

	void QueueEvent()
	{
		m_EventManager.QueueEvent( new EventManager.Event
		{
			m_Behaviour = m_ToGenerate,
			m_Duration = m_ToGenerate.CalculateDuration( m_DifficultyManager.Difficulty ),
			m_StartTime = m_TimeManager.m_CurrentTime + m_EventGenerationOffset,
		} );
	}

	float CalculateEventDelay()
	{
		//TODO: Figure out exactly what we want here. Some kind of inverse relationship between delay time and difficulty
		return m_InitialDelayBetweenEvents / m_DifficultyManager.Difficulty;
	}
}
