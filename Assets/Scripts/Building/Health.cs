using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
	public class OnDamageEvent : UnityEvent<Health> { }
	public GameObject m_fireSource;


	[SerializeField] float m_InitialHealthRatio = 1.0f;
	[SerializeField] float m_MaxHealth = 100.0f;

	[SerializeField] public OnDamageEvent m_OnDamageEvent = new OnDamageEvent();

	float m_Health;

	public float HealthRatio => m_Health / m_MaxHealth;

	private void Awake()
	{
		m_Health = m_MaxHealth * m_InitialHealthRatio;
		
	}

    private void Start()
    {
		if (m_fireSource != null)
		{
			m_fireSource.SetActive( false );
		}
	}

    public void Modify( float amount )
	{
		float newHealth = Mathf.Clamp( m_Health + amount, 0, m_MaxHealth );

		if (newHealth == m_Health)
		{
			return;
		}

		if (HealthRatio < 0.95)
		{
			if (m_fireSource != null)
			{
				m_fireSource.SetActive( true );
			}
		}
		else
        {
			if (m_fireSource != null)
			{
				m_fireSource.SetActive( false );
			}

		}
		m_Health = newHealth;

		m_OnDamageEvent?.Invoke( this );
	}

	public void FullHeal()
    {
		m_Health = m_MaxHealth;
		if (m_fireSource != null)
		{
			m_fireSource.SetActive( true );
		}
	}



}
