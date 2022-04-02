using UnityEngine;

public class MeteorManager : MonoBehaviour, IDisaster
{
	[SerializeField] Transform m_MeteorSpawnArea;
	[SerializeField] GameObject m_MeteorPrefab;
	[SerializeField] int m_MeteorsToSpawn;

	void IDisaster.TriggerDisaster()
	{
		for ( int i = 0; i < m_MeteorsToSpawn; ++i )
		{
			Vector3 position = GetSpawnLocation();
			Quaternion rotation = Random.rotation;

			GameObject meteor = GameObject.Instantiate( m_MeteorPrefab, position, rotation );
		}
	}

	Vector3 GetSpawnLocation()
	{
		Vector3 min = m_MeteorSpawnArea.position - m_MeteorSpawnArea.localScale / 2.0f;
		Vector3 max = m_MeteorSpawnArea.position + m_MeteorSpawnArea.localScale / 2.0f;

		Vector3 position = new Vector3( Random.Range( min.x, max.x ), Random.Range( min.y, max.y ), Random.Range( min.z, max.z ) );
		return position;
	}
}
