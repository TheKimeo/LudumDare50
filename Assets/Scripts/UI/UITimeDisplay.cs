using System.Collections.Generic;
using UnityEngine;

public class UITimeDisplay : MonoBehaviour
{
	[SerializeField] RectTransform m_Timeline;
	[SerializeField] RectTransform m_EventPrefab;
	[Space]
	[SerializeField] float m_TimelineDuration;

	TimeManager m_TimeManager;
	EventManager m_EventManager;

	Dictionary<EventManager.Event, RectTransform> m_TimeEvents = new Dictionary<EventManager.Event, RectTransform>();
	static List<EventManager.Event> s_RemoveBuffer = new List<EventManager.Event>();

	private void Start()
	{
		m_TimeManager = TimeManager.Instance;
		m_EventManager = EventManager.Instance;
	}

	private void LateUpdate()
	{
		List<EventManager.Event> queuedEvents = m_EventManager.m_QueuedEvents;
		List<EventManager.Event> completedEvents = m_EventManager.m_CompletedEvents;

		UpdateAllEvents( queuedEvents );
		UpdateAllEvents( completedEvents );

		RemoveExtraEvents( queuedEvents, completedEvents );
	}

	private void UpdateAllEvents( List<EventManager.Event> events )
	{
		for ( int i = 0; i < events.Count; ++i )
		{
			EventManager.Event e = events[ i ];
			if ( m_TimeEvents.TryGetValue( e, out RectTransform eventTransform ) == false )
			{
				eventTransform = AddTimeEvent( e );
			}

			UpdatePosition( eventTransform, e );
		}
	}

	private void RemoveExtraEvents( List<EventManager.Event> queuedEvents, List<EventManager.Event> completedEvents )
	{
		//This isn't very fast, but it works!
		s_RemoveBuffer.Clear();

		foreach ( KeyValuePair<EventManager.Event, RectTransform> eventPair in m_TimeEvents )
		{
			EventManager.Event e = eventPair.Key;
			if (queuedEvents.Contains( e ) == false && completedEvents.Contains( e ) == false )
			{
				s_RemoveBuffer.Add( e );
			}
		}

		foreach ( EventManager.Event e in s_RemoveBuffer )
		{
			RectTransform eventTransform = m_TimeEvents[ e ];

			GameObject.Destroy( eventTransform.gameObject );
			m_TimeEvents.Remove( e );
		}
	}

	private RectTransform AddTimeEvent( EventManager.Event e )
	{
		RectTransform newEvent = GameObject.Instantiate( m_EventPrefab, m_Timeline );
		m_TimeEvents.Add( e, newEvent );
		return newEvent;
	}

	private void UpdatePosition( RectTransform transform, EventManager.Event e )
	{
		float startX = GetTimelineRelativePosition( e.m_StartTime );
		float sizeX = GetTimelineRelativeSize( e.m_Duration );

		transform.SetInsetAndSizeFromParentEdge( RectTransform.Edge.Left, startX, sizeX );
	}

	float GetTimelineRelativePosition(float time)
	{
		float timeDifference = time - m_TimeManager.m_CurrentTime;
		float timeDifferenceRatio = timeDifference / m_TimelineDuration;

		float timelineWidth = m_Timeline.rect.width;
		return timeDifferenceRatio * timelineWidth;
	}

	float GetTimelineRelativeSize( float time )
	{
		float timeRatio = time / m_TimelineDuration;
		float timelineWidth = m_Timeline.rect.width;
		return timeRatio * timelineWidth;
	}
}
