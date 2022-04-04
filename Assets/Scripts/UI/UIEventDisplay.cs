using System.Collections.Generic;
using UnityEngine;

public class UIEventDisplay : MonoBehaviour
{
	[SerializeField] RectTransform m_EventParent;
	[SerializeField] UIEvent m_EventPrefab;
	[SerializeField] CanvasGroup m_CanvasGroup;

	TimeManager m_TimeManager;
	EventManager m_EventManager;
	VisibleTimelineManager m_TimelineManager;

	Dictionary<EventManager.Event, RectTransform> m_TimeEvents = new Dictionary<EventManager.Event, RectTransform>();
	static List<EventManager.Event> s_RemoveBuffer = new List<EventManager.Event>();

	private void Start()
	{
		m_TimeManager = TimeManager.Instance;
		m_EventManager = EventManager.Instance;
		m_TimelineManager = VisibleTimelineManager.Instance;

		m_CanvasGroup.alpha = 0.0f;
	}

	private void LateUpdate()
	{
		if ( m_TimelineManager.VisibleSeconds < 0.1f )
		{
			if ( m_CanvasGroup.alpha > 0.0f )
			{
				m_CanvasGroup.alpha = Mathf.MoveTowards( m_CanvasGroup.alpha, 0.0f, 2.0f * Time.deltaTime );
			}
			return;
		}
		else if ( m_CanvasGroup.alpha < 1.0f )
		{
			m_CanvasGroup.alpha = Mathf.MoveTowards( m_CanvasGroup.alpha, 1.0f, 2.0f * Time.deltaTime );
		}

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

			if ( e.m_Behaviour.m_VisibleOnTimeline == false )
			{
				continue;
			}

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
			if ( queuedEvents.Contains( e ) == false && completedEvents.Contains( e ) == false )
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
		UIEvent newEvent = GameObject.Instantiate( m_EventPrefab, m_EventParent );
		newEvent.Initialize( e );

		RectTransform rectTransform = newEvent.GetComponent<RectTransform>();
		m_TimeEvents.Add( e, rectTransform );

		return rectTransform;
	}

	private void UpdatePosition( RectTransform transform, EventManager.Event e )
	{
		float startX = GetTimelineRelativePosition( e.m_StartTime );
		float sizeX = GetTimelineRelativeSize( e.m_Duration );

		transform.SetInsetAndSizeFromParentEdge( RectTransform.Edge.Left, startX, sizeX );
	}

	float GetTimelineRelativePosition( float time )
	{
		float timeDifference = time - m_TimeManager.m_CurrentTime;
		float timeDifferenceRatio = timeDifference / m_TimelineManager.VisibleSeconds;

		float timelineWidth = m_EventParent.rect.width;
		return timeDifferenceRatio * timelineWidth;
	}

	float GetTimelineRelativeSize( float time )
	{
		float timeRatio = time / m_TimelineManager.VisibleSeconds;
		float timelineWidth = m_EventParent.rect.width;
		return timeRatio * timelineWidth;
	}
}
