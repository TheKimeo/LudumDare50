using UnityEngine;

public class MeteorManager : MonoBehaviour, IDisaster
{
	[SerializeField] GameObject m_MeteorPrefab;
	[SerializeField] float m_MeteorSpawnRadius;
	[SerializeField] float m_MeteorSpawnHeight;
	[SerializeField] float m_DelayBetweenMeteors;
	[SerializeField] float m_MeteorFrequencyDifficultyScale = 1.0f;
	[SerializeField] AudioClip m_stormSound ;

	float m_StopMeteorTime;
	float m_NextMeteorTime;
	AudioSource m_audioSource;

	void Start()
	{
		m_StopMeteorTime = -1.0f;
		m_NextMeteorTime = -1.0f;
		m_audioSource = GetComponent<AudioSource>();
		Debug.Assert( m_DelayBetweenMeteors >= 0.01f, "[MeteorManager] Must have a delay >= 0.01 between meteors or the game will lag out and die" );
	}

	void Update()
	{
		float time = Time.time;
		if ( m_StopMeteorTime < time )
		{
			//Stop audio here
			m_audioSource.Pause();
			return;
		}

	

		DifficultyManager difficultyManager = DifficultyManager.Instance;
		float difficulty = difficultyManager.Difficulty;
		float scaledDifficulty = difficulty * m_MeteorFrequencyDifficultyScale;

		while ( m_NextMeteorTime <= time )
		{
			m_NextMeteorTime += m_DelayBetweenMeteors / scaledDifficulty;
			SpawnMeteor();
		}
	}

	void SpawnMeteor()
	{
		Vector3 position = GetSpawnLocation();
		Quaternion rotation = Random.rotation;

		GameObject.Instantiate( m_MeteorPrefab, position, rotation );
	}

	Vector3 GetSpawnLocation()
	{
		Vector2 spawnPosition = Random.insideUnitCircle* m_MeteorSpawnRadius;
		return new Vector3( spawnPosition.x, m_MeteorSpawnHeight, spawnPosition.y );
	}

	void IDisaster.TriggerDisaster( float duration )
	{
		if (!m_audioSource.isPlaying)
		{
			m_audioSource.Play();
		}
		float time = Time.time;
		m_StopMeteorTime = time + duration;
		m_NextMeteorTime = time + m_DelayBetweenMeteors;
	}
}
