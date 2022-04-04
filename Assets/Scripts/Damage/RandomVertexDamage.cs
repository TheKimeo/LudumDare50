using System.Collections.Generic;
using UnityEngine;

public class RandomVertexDamage : MonoBehaviour
{
	static List<Vector3> s_VertexBuffer = new List<Vector3>();
	static List<MeshFilter> s_FilterBuffer = new List<MeshFilter>();

	[SerializeField] int m_MaxDamagePoints = 15;
	[SerializeField] GameObject m_DamagePrefab;
	Health m_Health;

	List<GameObject> m_SpawnedPrefabs = new List<GameObject>();

	private void Awake()
	{
		string failedMeshes = VerifyMeshFilters();
		if ( string.IsNullOrEmpty( failedMeshes ) == false )
		{
			Debug.Assert( false, "[RandomVertextDamage] Object " + gameObject + " has non-readable meshs:\n" + failedMeshes, gameObject );
			return;
		}

		m_Health = GetComponent<Health>();
	}

	private void Start()
	{
		if ( m_Health != null )
		{
			m_Health.m_OnDamageEvent.AddListener( OnDamaged );
			SetDamage( 1.0f - m_Health.HealthRatio );
		}
	}

	private void OnEnable()
	{
		if (m_Health != null)
		{
			float damageRatio = 1.0f - m_Health.HealthRatio;
			SetDamage( damageRatio );
		}
		else
		{
			SetDamage( 1.0f );
		}
	}

	private void OnDisable()
	{
		SetDamage( 0.0f );
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

	public string VerifyMeshFilters()
	{
		s_FilterBuffer.Clear();
		GetComponentsInChildren<MeshFilter>( s_FilterBuffer );

		string failures = "";

		foreach (MeshFilter filter in s_FilterBuffer)
		{
			Mesh mesh = filter.sharedMesh;
			if ( mesh == null )
			{
				continue;
			}

			if ( mesh.isReadable == false )
			{
				failures += mesh.name + "\n";
			}
		}

		return failures;
	}
}
