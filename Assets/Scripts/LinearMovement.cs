using UnityEngine;

public class LinearMovement : MonoBehaviour
{
	[SerializeField] Rigidbody m_Rigidbody;
	[SerializeField] Vector3 m_Velocity;

	private void Start()
	{
		m_Rigidbody.AddRelativeForce( m_Velocity, ForceMode.Impulse );
	}
}