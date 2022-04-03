using UnityEngine;

public interface IDisaster
{
	void TriggerDisaster(float duration);
}

public class DisasterManager : Singleton<DisasterManager>
{
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
		return difficulty * 3.0f;
	}

	IDisaster SelectDisaster()
	{
		int index = Random.Range( 0, m_Disasters.Length );
		return m_Disasters[ index ];
	}
}