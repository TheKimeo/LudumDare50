using UnityEngine;

public class MeteorManager : MonoBehaviour, IDisaster
{
	[SerializeField] Transform m_MeteorSpawnArea;
	[SerializeField] GameObject m_MeteorPrefab;
	[SerializeField] float m_DelayBetweenMeteors;

	float m_StopMeteorTime;
	float m_NextMeteorTime;

	void Start()
	{
		m_StopMeteorTime = -1.0f;
		m_NextMeteorTime = -1.0f;
	}

	void Update()
	{
		float time = Time.time;
		if ( m_StopMeteorTime < time )
		{
			//TODO: Maybe disable this when not needed?
			return;
		}

		if ( m_NextMeteorTime > time )
		{
			return;
		}

		m_NextMeteorTime = time + m_DelayBetweenMeteors;
		SpawnMeteor();
	}

	void SpawnMeteor()
	{
		Vector3 position = GetSpawnLocation();
		Quaternion rotation = Random.rotation;

		GameObject.Instantiate( m_MeteorPrefab, position, rotation );
	}

	Vector3 GetSpawnLocation()
	{
		Vector3 min = m_MeteorSpawnArea.position - m_MeteorSpawnArea.localScale / 2.0f;
		Vector3 max = m_MeteorSpawnArea.position + m_MeteorSpawnArea.localScale / 2.0f;

		Vector3 position = new Vector3( Random.Range( min.x, max.x ), Random.Range( min.y, max.y ), Random.Range( min.z, max.z ) );
		return position;
	}

	void IDisaster.TriggerDisaster( float duration )
	{
		float time = Time.time;
		m_StopMeteorTime = time + duration;
		m_NextMeteorTime = time + m_DelayBetweenMeteors;
	}
}
