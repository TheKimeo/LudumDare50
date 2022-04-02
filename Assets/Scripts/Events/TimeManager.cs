using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
	[SerializeField] float m_DayDuration;
	[SerializeField] float m_NightDuration;

	[HideInInspector] public float m_CurrentTime;

	public float CycleDuration => m_DayDuration + m_NightDuration;
	public int Cycle => Mathf.FloorToInt( m_CurrentTime / CycleDuration );
	public float CycleDayStartTime => Cycle * CycleDuration;
	public float CycleNightStartTime => Cycle * CycleDuration + m_DayDuration;

	public float DayRatio => ( m_CurrentTime - CycleDayStartTime ) / m_DayDuration;
	public float NightRatio => ( m_CurrentTime - CycleNightStartTime ) / m_NightDuration;
	public bool IsDay => m_CurrentTime < CycleNightStartTime;

	private void Start()
	{
		m_CurrentTime = 0.0f;
	}

	private void Update()
	{
		m_CurrentTime += Time.deltaTime;
	}
}
