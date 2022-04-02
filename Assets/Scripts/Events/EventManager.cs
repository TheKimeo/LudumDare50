using System.Collections.Generic;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
	public struct Event : IEqualityComparer<Event>
	{
		public EventBehaviour m_Behaviour;
		public float m_StartTime;
		public float m_Duration;

		public bool Equals( Event x, Event y )
		{
			return x.m_Behaviour == y.m_Behaviour
				&& x.m_StartTime == y.m_StartTime
				&& x.m_Duration == y.m_Duration;
		}

		public int GetHashCode( Event obj ) => m_Behaviour.GetHashCode() ^ m_StartTime.GetHashCode() ^ m_Duration.GetHashCode();
	}

	[SerializeField] float m_DelayBeforeRemoveEvent = 10;
	[Space]
	[SerializeField] EventBehaviour m_TestBehaviour;
	[SerializeField] float m_TestDelay;
	[SerializeField] float m_TestDuration;

	//Ordered by m_StartTime. Earliest to start first.
	public List<Event> m_QueuedEvents = new List<Event>();
	public List<Event> m_CompletedEvents = new List<Event>();

	private void Start()
	{
		QueueEvent( new Event
		{
			m_Behaviour = m_TestBehaviour,
			m_StartTime = TimeManager.Instance.m_CurrentTime + m_TestDelay,
			m_Duration = m_TestDuration,
		} );
	}

	private void Update()
	{
		TimeManager timeManager = TimeManager.Instance;
		float currentTime = timeManager.m_CurrentTime;

		//Process and start any queued events whose start times have now passed
		while ( m_QueuedEvents.Count > 0 )
		{
			Event e = m_QueuedEvents[ 0 ];
			if ( e.m_StartTime > currentTime )
			{
				break;
			}

			StartEvent( e );
			m_QueuedEvents.RemoveAt( 0 ); //Expensive, maybe move to deque instead of list if too slow
		}

		//Remove events that have ended a long time ago
		for ( int i = m_CompletedEvents.Count - 1; i >= 0; --i )
		{
			Event e = m_CompletedEvents[ i ];
			float endTime = e.m_StartTime + e.m_Duration;
			float timeSinceEnd = currentTime - endTime;

			if ( timeSinceEnd < m_DelayBeforeRemoveEvent )
			{
				continue;
			}

			m_CompletedEvents.RemoveAt( i );
		}
	}

	public void StartEvent( Event e )
	{
		e.m_Behaviour.ApplyEvent( e );
		m_CompletedEvents.Add( e );
	}

	public void QueueEvent( Event newEvent )
	{
		Debug.Assert( newEvent.m_StartTime >= TimeManager.Instance.m_CurrentTime );

		//Keep ordered
		for ( int i = 0; i < m_QueuedEvents.Count; ++i )
		{
			Event e = m_QueuedEvents[ i ];
			if ( e.m_StartTime <= newEvent.m_StartTime )
			{
				continue;
			}

			m_QueuedEvents.Insert( i, newEvent );
		}

		m_QueuedEvents.Add( newEvent );
	}
}
