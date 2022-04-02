using UnityEngine;

[CreateAssetMenu( fileName = "ResourceDefinition", menuName = "Resources/Resource", order = 0 )]
public class Resource : ScriptableObject
{
	[SerializeField] public float m_InitialValue;
	[SerializeField] public float m_MinValue;
	[SerializeField] public float m_MaxValue;
	[SerializeField] public Notification m_insufficientNotif;


	[HideInInspector] public float m_Value;

	public void Initialise()
	{
		Debug.Assert( m_InitialValue >= m_MinValue );
		Debug.Assert( m_InitialValue <= m_MaxValue );

		m_Value = m_InitialValue;
	}

	public bool CanConsume( float amount )
	{
		return ( m_Value - m_MinValue ) >= amount;
	}

	public void Modify( float amount )
	{
		float newValue = m_Value + amount;
		m_Value = Mathf.Clamp( newValue, m_MinValue, m_MaxValue );
	}
}
