using UnityEngine;

public class EnvironmentDistributer : MonoBehaviour
{
	[SerializeField] float m_OuterRadius = 70.0f;
	[SerializeField] float m_InnerRadius = 5.0f;
	[SerializeField] float m_Density;
	[SerializeField] EnvironmentPrefab[] m_Decorations;

	void Awake()
	{
		float totalWeighting = CalculateTotalWeighting();

		float outterArea = Mathf.PI * m_OuterRadius * m_OuterRadius;
		float toSpawnCount = outterArea * m_Density / 20.0f; //Magic number to help with input scales

		float innerRadiusSqr = m_InnerRadius * m_InnerRadius;

		for ( int i = 0; i < toSpawnCount; ++i )
		{
			Vector2 xzPosition = Random.insideUnitCircle * m_OuterRadius;
			if ( xzPosition.SqrMagnitude() < innerRadiusSqr )
			{
				//Skip points in the inner circle. We are calculating the count from density so this should still give an even covering
				continue;
			}

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
		Debug.Assert( m_Decorations.Length > 0, gameObject );

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
