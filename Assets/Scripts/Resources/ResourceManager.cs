using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : Singleton<ResourceManager>
{
	[SerializeField] Resource[] m_Resources;
	[SerializeField] float m_TickDelay = 1.0f;

	public UnityAction m_OnTickEvent;

	float m_TimeToTick;

	protected override void Awake()
	{
		base.Awake();

		for ( int i = 0; i < m_Resources.Length; ++i )
		{
			m_Resources[ i ].Initialise();
		}

		m_TimeToTick = m_TickDelay;
	}

	private void Update()
	{
		m_TimeToTick -= Time.deltaTime;

		while (m_TimeToTick <= 0.0f)
		{
			m_TimeToTick += m_TickDelay;

			DoTick();
		}
	}

	private void DoTick()
	{
		foreach (Resource resource in m_Resources)
		{
			resource.m_BeforeTick = resource.m_Value;
		}

		m_OnTickEvent?.Invoke();

		foreach ( Resource resource in m_Resources )
		{
			resource.m_AfterTick = resource.m_Value;
		}
	}
}
