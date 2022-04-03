using UnityEngine;

[CreateAssetMenu( fileName = "ResourceDefinition", menuName = "Resources/Resource", order = 0 )]
public class Resource : ScriptableObject
{
	[SerializeField] private float m_InitialValue;
	[SerializeField] private float m_MinValue;
	[SerializeField] private float m_MaxValue;

	[SerializeField] public Notification m_insufficientNotif;
	[SerializeField] public Sprite m_Icon;


	[HideInInspector] public float m_Value;
	[HideInInspector] public float m_Min;
	[HideInInspector] public float m_Max;

	public void Initialise()
	{
		Debug.Assert( m_InitialValue >= m_MinValue );
		Debug.Assert( m_InitialValue <= m_MaxValue );

		m_Value = m_InitialValue;
		m_Min = m_MinValue;
		m_Max = m_MaxValue;
	}

	public bool CanConsume( float amount )
	{
		return ( m_Value - m_MinValue ) >= amount;
	}

	public void Modify( float amount )
	{
		float newValue = m_Value + amount;
		m_Value = Mathf.Clamp( newValue, m_MinValue, m_Max );
	}
}
