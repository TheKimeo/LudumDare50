using UnityEngine;

public class DaylightManager : MonoBehaviour
{
	[SerializeField] Light m_Sun;
	[SerializeField] Light m_Moon;
	
	[SerializeField] Vector3 m_SunRotationAxis;
	[SerializeField] AnimationCurve m_LightIntensity;

	private void Start()
	{
		m_SunRotationAxis.Normalize();
		Debug.Assert( m_SunRotationAxis != Vector3.zero, gameObject );
	}

	private void LateUpdate()
	{
		TimeManager timeManager = TimeManager.Instance;
		m_Sun.intensity = m_LightIntensity.Evaluate( timeManager.DayRatio );
		m_Moon.intensity = m_LightIntensity.Evaluate( timeManager.NightRatio );

		float angle = GetSunAngle();
		Quaternion sunRotation = Quaternion.AngleAxis( angle, m_SunRotationAxis );
		Quaternion moonRotation = Quaternion.AngleAxis( angle + 180.0f, m_SunRotationAxis );

		m_Sun.transform.rotation = sunRotation;
		m_Moon.transform.rotation = moonRotation;

		bool isDay = timeManager.IsDay;

		m_Sun.gameObject.SetActive( isDay );
		m_Moon.gameObject.SetActive( isDay == false );
	}

	private float GetSunAngle()
	{
		TimeManager timeManager = TimeManager.Instance;

		if ( timeManager.IsDay )
		{
			return Mathf.Lerp( 0.0f, 180.0f, timeManager.DayRatio );
		}
		else
		{
			return Mathf.Lerp( 180.0f, 360.0f, timeManager.NightRatio );
		}
	}
}
