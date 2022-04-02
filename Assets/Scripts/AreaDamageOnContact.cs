using UnityEngine;

public class AreaDamageOnContact : MonoBehaviour
{
	[SerializeField] ParticleSystem m_ExplosionPrefab;
	[SerializeField] float m_Damage;

	private void OnCollisionEnter( Collision collision )
	{
		Debug.Assert( collision.contactCount > 0 );

		Health health = collision.gameObject.GetComponent<Health>();
		if ( health != null )
		{
			ApplyDamage( health );
		}

		ContactPoint contact = collision.GetContact(0);
		SpawnExplosion( contact.point );

		GameObject.Destroy( gameObject );
	}

	void ApplyDamage( Health health )
	{
		health.Modify( -m_Damage );
	}

	void SpawnExplosion( Vector3 position )
	{
		GameObject.Instantiate( m_ExplosionPrefab, position, Quaternion.identity );
	}
}
