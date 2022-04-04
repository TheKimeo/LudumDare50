using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : Singleton<ResourceManager>
{
	public interface IResourceModifier
	{
		void OnGainTick( Resource resource, ref float currentCumulative );
		void OnLossTick( Resource resource, ref float currentCumulative );
	}

	[SerializeField] Resource[] m_Resources;
	[SerializeField] float m_TickDelay = 1.0f;

	public List<IResourceModifier> m_OnTickEvent = new List<IResourceModifier>();

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
		foreach ( Resource resource in m_Resources )
		{
			float cumulative = 0.0f;
			for ( int i = 0; i < m_OnTickEvent.Count; ++i )
			{
				m_OnTickEvent[ i ].OnGainTick( resource, ref cumulative );
			}

			for ( int i = 0; i < m_OnTickEvent.Count; ++i )
			{
				m_OnTickEvent[ i ].OnLossTick( resource, ref cumulative );
			}

			resource.m_DifferencePerTick = cumulative;
			resource.Modify( cumulative );
		}
	}
}
