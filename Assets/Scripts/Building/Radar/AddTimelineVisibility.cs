using UnityEngine;

public class AddTimelineVisibility : MonoBehaviour
{
	[SerializeField] float m_AddAmount = 1.0f;
	[SerializeField] BuildingState m_State;

	bool m_AddedVisibility = false;

	private void OnEnable()
	{
		AddVisibility();
	}

	private void OnDisable()
	{
		RemoveVisibility();
	}

	private void Update()
	{
		if ( m_State.OperationalRatio < 0.1f )
		{
			RemoveVisibility();
		}
		else
		{
			AddVisibility();
		}
	}

	void AddVisibility()
	{
		if ( m_AddedVisibility )
		{
			return;
		}

		VisibleTimelineManager visibilityManager = VisibleTimelineManager.Instance;
		visibilityManager?.ModifyVisibility( m_AddAmount );
		m_AddedVisibility = true;
	}

	void RemoveVisibility()
	{
		if ( m_AddedVisibility == false )
		{
			return;
		}

		VisibleTimelineManager visibilityManager = VisibleTimelineManager.Instance;
		visibilityManager?.ModifyVisibility( -m_AddAmount );
		m_AddedVisibility = false;
	}
}
