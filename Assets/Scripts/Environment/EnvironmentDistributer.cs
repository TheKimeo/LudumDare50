using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDistributer : MonoBehaviour
{
	[System.Serializable]
	public struct Layer
	{
		[SerializeField] public float m_Density;
		[SerializeField] public float m_BlockedRadius;
		[SerializeField] public bool m_AllowIntersectWithinLayer;
		[SerializeField] public float m_InnerRadius;
		[SerializeField] public float m_OuterRadius;
		[SerializeField] public EnvironmentPrefab[] m_Decorations;
	}

	[SerializeField] Layer[] m_Layers;

	struct Blocker
	{
		public Vector2 m_Position;
		public float m_Radius;
	}

	List<Blocker> m_BlockedPositions = new List<Blocker>();

	void Awake()
	{
		m_BlockedPositions.Clear();
		foreach ( Layer layer in m_Layers )
		{
			ProcessLayer( layer, m_BlockedPositions );
		}
	}

	void ProcessLayer( Layer layer, List<Blocker> blockers )
	{
		float totalWeighting = CalculateTotalWeighting( layer.m_Decorations );

		float outterArea = Mathf.PI * layer.m_OuterRadius * layer.m_OuterRadius;
		int toSpawnCount = Mathf.RoundToInt( outterArea * layer.m_Density / 20.0f ); //Magic number to help with input scales

		float innerRadiusSqr = layer.m_InnerRadius * layer.m_InnerRadius;

		int initialBlockersSize = blockers.Count;

		for ( int i = 0; i < toSpawnCount; ++i )
		{
			Vector2 xzPosition = Random.insideUnitCircle * layer.m_OuterRadius;
			if ( xzPosition.SqrMagnitude() < innerRadiusSqr )
			{
				//Skip points in the inner circle. We are calculating the count from density so this should still give an even covering
				continue;
			}

			int blockersSize = layer.m_AllowIntersectWithinLayer
				? initialBlockersSize
				: blockers.Count;

			EnvironmentPrefab environmentPrefab = GetRandomPrefab( layer.m_Decorations, totalWeighting );
			float scale = Random.Range( environmentPrefab.m_MinScale, environmentPrefab.m_MaxScale );

			if ( IntersectsBlocker( xzPosition, layer.m_BlockedRadius * scale, blockers, blockersSize ) )
			{
				//Skip as it intersects with an existing blocking environment asset
				continue;
			}

			Vector3 position = new Vector3( xzPosition.x, 0.0f, xzPosition.y );

			float randomRotation = Random.Range( 0.0f, 360.0f );
			Quaternion rotation = Quaternion.AngleAxis( randomRotation, Vector3.up );

			GameObject spawnedPrefab = GameObject.Instantiate( environmentPrefab.m_Prefab, position, rotation );
			spawnedPrefab.transform.localScale = Vector3.one * scale;

			blockers.Add( new Blocker
			{
				m_Position = new Vector2( position.x, position.z ),
				m_Radius = layer.m_BlockedRadius,
			} );
		}
	}

	bool IntersectsBlocker( Vector2 position, float radius, List<Blocker> blockers, int count )
	{
		for (int i = 0; i < count; ++i )
		{
			Blocker blocker = blockers[ i ];
			float distanceSqr = ( position - blocker.m_Position ).sqrMagnitude;
			float totalRadius = blocker.m_Radius + radius;
			if ( distanceSqr <= ( totalRadius * totalRadius ) )
			{
				return true;
			}
		}

		return false;
	}

	EnvironmentPrefab GetRandomPrefab( EnvironmentPrefab[] prefabs, float totalWeighting )
	{
		Debug.Assert( prefabs.Length > 0, gameObject );

		float remainingWeight = Random.Range( 0.0f, totalWeighting );
		foreach (EnvironmentPrefab environmentPrefab in prefabs )
		{
			remainingWeight -= environmentPrefab.m_Weighting;

			if ( remainingWeight <= 0.0f )
			{
				return environmentPrefab;
			}
		}

		return prefabs[ prefabs.Length - 1 ];
	}

	float CalculateTotalWeighting( EnvironmentPrefab[] prefabs )
	{
		float weighting = 0.0f;
		foreach ( EnvironmentPrefab environmentPrefab in prefabs )
		{
			weighting += environmentPrefab.m_Weighting;
		}
		return weighting;
	}
}
