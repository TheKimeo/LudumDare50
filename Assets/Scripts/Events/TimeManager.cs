using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
	[SerializeField] float m_DayDuration = 30.0f;
	[SerializeField] float m_NightDuration = 30.0f;
	[SerializeField] float m_InitialTime = 0.0f;

	[HideInInspector] public float m_CurrentTime;

	public float CycleDuration => m_DayDuration + m_NightDuration;
	public int Cycle => Mathf.FloorToInt( m_CurrentTime / CycleDuration );
	public float CycleDayStartTime => Cycle * CycleDuration;
	public float CycleNightStartTime => Cycle * CycleDuration + m_DayDuration;

	public float DayRatio => ( m_CurrentTime - CycleDayStartTime ) / m_DayDuration;
	public float NightRatio => ( m_CurrentTime - CycleNightStartTime ) / m_NightDuration;
	public bool IsDay => m_CurrentTime < CycleNightStartTime;

	protected override void Awake()
	{
		base.Awake();

		m_CurrentTime = m_InitialTime;
	}

	private void Update()
	{
		m_CurrentTime += Time.deltaTime;
	}
}
