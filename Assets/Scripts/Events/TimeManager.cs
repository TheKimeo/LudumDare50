using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
	[SerializeField] float m_DayLength;
	[SerializeField] float m_NightLength;

	[HideInInspector] public float m_CurrentTime;

	public float WholeDayLength => m_DayLength + m_NightLength;

	public float CycleRatio => m_CurrentTime % WholeDayLength;
	public int Cycle => Mathf.FloorToInt( m_CurrentTime / WholeDayLength );

	private void Start()
	{
		m_CurrentTime = 0.0f;
	}

	private void Update()
	{
		m_CurrentTime += Time.deltaTime;
	}
}
