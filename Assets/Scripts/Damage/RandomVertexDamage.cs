using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof( Health ) )]
public class RandomVertexDamage : MonoBehaviour
{
	static List<Vector3> s_VertexBuffer = new List<Vector3>();
	static List<MeshFilter> s_FilterBuffer = new List<MeshFilter>();

	[SerializeField] int m_MaxDamagePoints = 15;
	[SerializeField] GameObject m_DamagePrefab;
	[SerializeField] Health m_Health;

	List<GameObject> m_SpawnedPrefabs = new List<GameObject>();

	private void Start()
	{
		if ( m_Health == null )
		{
			Debug.Assert( false, "[RandomVertextDamage] Object " + gameObject + " has no Health component?" );
			return;
		}

		m_Health.m_OnDamageEvent.AddListener( OnDamaged );
	}

	private void OnDamaged( Health health )
	{
		float damageRatio = 1.0f - health.HealthRatio;
		SetDamage( damageRatio );
	}

	// 0 for no damage, 1 for maximum damage
	public void SetDamage( float ratio )
	{
		int numParticles = Mathf.RoundToInt( (float) m_MaxDamagePoints * ratio );

		if ( numParticles > m_SpawnedPrefabs.Count )
		{
			//Spawn more particles
			for ( int i = m_SpawnedPrefabs.Count; i < numParticles; ++i )
			{
				GameObject damagePrefab = GameObject.Instantiate( m_DamagePrefab, transform );
				damagePrefab.transform.position = GetRandomPosition();
				m_SpawnedPrefabs.Add( damagePrefab );
			}
		}
		else
		{
			//Remove excess particles
			for ( int i = m_SpawnedPrefabs.Count - 1; i >= numParticles; --i )
			{
				GameObject.Destroy( m_SpawnedPrefabs[ i ] );
				m_SpawnedPrefabs.RemoveAt( i );
			}
		}
	}

	public Vector3 GetRandomPosition()
	{
		s_VertexBuffer.Clear();
		s_FilterBuffer.Clear();

		GetComponentsInChildren<MeshFilter>( s_FilterBuffer );

		MeshFilter filter = s_FilterBuffer[ Random.Range( 0, s_FilterBuffer.Count ) ];
		filter.sharedMesh.GetVertices( s_VertexBuffer );

		Vector3 randomPosition = s_VertexBuffer[ Random.Range( 0, s_VertexBuffer.Count ) ];

		Matrix4x4 localToWorld = filter.transform.localToWorldMatrix;
		return localToWorld.MultiplyPoint3x4( randomPosition );
	}
}
