using UnityEngine;

[RequireComponent(typeof(Health))]
public class DestroyOnDeath : MonoBehaviour
{
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

		GameObject.Destroy( gameObject );
	}
}
