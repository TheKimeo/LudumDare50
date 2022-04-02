using UnityEngine;

public interface IDisaster
{
	void TriggerDisaster();
}

public class DisasterManager : Singleton<DisasterManager>
{
	[SerializeField] MeteorManager m_MeteorManager;
	[SerializeField] float m_Config_DelayBeforeDisaster;

	IDisaster[] m_Disasters;
	float m_DelayBeforeDisaster;

	void Start()
    {
		m_Disasters = new IDisaster[]
		{
			m_MeteorManager,
		};

		m_DelayBeforeDisaster = m_Config_DelayBeforeDisaster;
	}

    // Update is called once per frame
    void Update()
    {
		m_DelayBeforeDisaster -= Time.deltaTime;

		if (m_DelayBeforeDisaster <= 0.0f)
		{
			m_DelayBeforeDisaster += m_Config_DelayBeforeDisaster;

			TriggerDisaster();
		}
	}

	public void TriggerDisaster()
	{
		IDisaster disaster = SelectDisaster();
		disaster.TriggerDisaster();
	}

	IDisaster SelectDisaster()
	{
		int index = Random.Range( 0, m_Disasters.Length );
		return m_Disasters[ index ];
	}
}
