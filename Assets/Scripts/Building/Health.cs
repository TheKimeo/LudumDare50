using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	public class OnDamageEvent : UnityEvent<Health> { }

	[SerializeField] float m_InitialHealthRatio = 1.0f;
	[SerializeField] float m_MaxHealth = 100.0f;

	[SerializeField] public OnDamageEvent m_OnDamageEvent;

	float m_Health;

	public float HealthRatio => m_Health / m_MaxHealth;

	private void Awake()
	{
		m_Health = m_MaxHealth * m_InitialHealthRatio;
	}

	public void Modify( float amount )
	{
		m_Health = Mathf.Clamp( m_Health + amount, 0, m_MaxHealth );

		m_OnDamageEvent?.Invoke( this );
	}
}
