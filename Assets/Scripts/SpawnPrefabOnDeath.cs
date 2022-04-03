using UnityEngine;

[RequireComponent(typeof(Health))]
public class SpawnPrefabOnDeath : MonoBehaviour
{
	[SerializeField] GameObject m_PrefabToSpawn;

	private void OnEnable()
	{
		Health health = GetComponent<Health>();
		health.m_OnDamageEvent.AddListener( OnDamageEvent );
	}

	private void OnDisable()
	{
		Health health = GetComponent<Health>();
		health.m_OnDamageEvent.RemoveListener( OnDamageEvent );
	}

	void OnDamageEvent( Health health )
	{
		if (health.HealthRatio != 0.0f )
		{
			return;
		}

		Vector3 position = transform.position;
		Quaternion rotation = transform.rotation;
		GameObject.Instantiate( m_PrefabToSpawn, position, rotation );
	}
}
