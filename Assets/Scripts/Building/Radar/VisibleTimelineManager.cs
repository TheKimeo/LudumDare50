using UnityEngine;

public class VisibleTimelineManager : Singleton<VisibleTimelineManager>
{
	[SerializeField] float m_VisiblityOutsideMultiplier = 12.0f;
	[SerializeField] float m_VisiblityInsideMultiplier = 2.0f;

	float m_TotalVisibility = 0.0f;

	public float VisibleSeconds => Mathf.Log( Mathf.Pow( m_TotalVisibility, m_VisiblityInsideMultiplier )+ 1.0f ) * m_VisiblityOutsideMultiplier;

	public void ModifyVisibility( float visibility )
	{
		m_TotalVisibility += visibility;
	}
}
