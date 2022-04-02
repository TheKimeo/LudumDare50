using UnityEngine;

public class EnvironmentDistributer : MonoBehaviour
{
	[SerializeField] float m_Radius;
	[SerializeField] float m_Density;
	[SerializeField] EnvironmentPrefab[] m_Decorations;

	void Awake()
	{
		float totalWeighting = CalculateTotalWeighting();

		float area = Mathf.PI * m_Radius * m_Radius;
		float toSpawnCount = area * m_Density / 20.0f; //Magic number to help with input scales

		for ( int i = 0; i < toSpawnCount; ++i )
		{
			Vector2 xzPosition = Random.insideUnitCircle * m_Radius;
			Vector3 position = new Vector3( xzPosition.x, 0.0f, xzPosition.y );

			float randomRotation = Random.Range( 0.0f, 360.0f );
			Quaternion rotation = Quaternion.AngleAxis( randomRotation, Vector3.up );

			EnvironmentPrefab environmentPrefab = GetRandomPrefab( totalWeighting );

			GameObject spawnedPrefab = GameObject.Instantiate( environmentPrefab.m_Prefab, position, rotation );
			spawnedPrefab.transform.localScale = Vector3.one * Random.Range( environmentPrefab.m_MinScale, environmentPrefab.m_MaxScale );
		}
	}

	EnvironmentPrefab GetRandomPrefab( float totalWeighting )
	{
		Debug.Assert( m_Decorations.Length > 0 );

		float remainingWeight = Random.Range( 0.0f, totalWeighting );
		foreach (EnvironmentPrefab environmentPrefab in m_Decorations )
		{
			remainingWeight -= environmentPrefab.m_Weighting;

			if ( remainingWeight <= 0.0f )
			{
				return environmentPrefab;
			}
		}

		return m_Decorations[ m_Decorations.Length - 1 ];
	}

	float CalculateTotalWeighting()
	{
		float weighting = 0.0f;
		foreach ( EnvironmentPrefab environmentPrefab in m_Decorations )
		{
			weighting += environmentPrefab.m_Weighting;
		}
		return weighting;
	}
}
