using UnityEngine;

public class AreaDamageOnContact : MonoBehaviour
{
	[SerializeField] ParticleSystem m_ExplosionPrefab;

	private void OnCollisionEnter( Collision collision )
	{
		Debug.Assert( collision.contactCount > 0 );

		ContactPoint contact = collision.GetContact(0);
		GameObject.Instantiate( m_ExplosionPrefab, contact.point, Quaternion.identity );

		GameObject.Destroy( gameObject );
	}
}
