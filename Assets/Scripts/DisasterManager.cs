using UnityEngine;

public interface IDisaster
{
	void TriggerDisaster(float duration);
}

public class DisasterManager : Singleton<DisasterManager>
{
	[SerializeField] float m_DurationDifficultyScale = 1.0f;
	[SerializeField] MeteorManager m_MeteorManager;

	IDisaster[] m_Disasters;

	protected override void Awake()
    {
		base.Awake();

		m_Disasters = new IDisaster[]
		{
			m_MeteorManager,
		};
	}

	public void TriggerDisaster( float duration )
	{
		IDisaster disaster = SelectDisaster();
		disaster.TriggerDisaster( duration );
	}

	public float CalculateDisasterDuration( float difficulty )
	{
		return difficulty * m_DurationDifficultyScale;
	}

	IDisaster SelectDisaster()
	{
		int index = Random.Range( 0, m_Disasters.Length );
		return m_Disasters[ index ];
	}
}