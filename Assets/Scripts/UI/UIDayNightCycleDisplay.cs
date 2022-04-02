using UnityEngine;

public class UIDayNightCycleDisplay : MonoBehaviour
{
	[SerializeField] UITimeline m_Timeline;
	[SerializeField] RectTransform m_DayNightCycleImage;

	private void Start()
	{
		float width = m_DayNightCycleImage.rect.width;
		m_DayNightCycleImage.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal );
	}

	private void LateUpdate()
	{
		
	}
}
