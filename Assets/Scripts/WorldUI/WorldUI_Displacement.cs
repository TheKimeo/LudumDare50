using UnityEngine;

public class WorldUI_Displacement : MonoBehaviour
{
	[SerializeField] Vector3 m_Axis;
	[SerializeField] float m_MaxMagnitude;
	[SerializeField] float m_Speed;

	Vector3 m_StartingLocalPosition;
	float m_Magnitude;
	float m_TargetMagnitude;

	private void Start()
	{
		m_StartingLocalPosition = transform.localPosition;
		m_Magnitude = 0.0f;
		m_TargetMagnitude = 0.0f;
	}

	private void Update()
	{
		m_Magnitude = Mathf.MoveTowards( m_Magnitude, m_TargetMagnitude, m_Speed * Time.deltaTime );
		transform.localPosition = m_StartingLocalPosition + m_Axis * m_Magnitude;
	}

	public void Apply()
	{
		m_TargetMagnitude = m_MaxMagnitude;
	}

	public void Remove()
	{
		m_TargetMagnitude = 0.0f;
	}

	public void Toggle()
	{
		m_TargetMagnitude = m_MaxMagnitude - m_TargetMagnitude;
	}
}
