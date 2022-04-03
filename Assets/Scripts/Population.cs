using UnityEngine;

[CreateAssetMenu(fileName = "PopulationDefinition", menuName = "Population", order = 0)]
public class Population : ScriptableObject
{
	[SerializeField] public float m_initialValue;
	[SerializeField] public float m_initialCap;


	[HideInInspector] public float m_Value;
	[HideInInspector] public float m_cap;


	public void Initialise()
	{
		Debug.Assert(m_initialValue <= m_initialCap);

		m_Value = m_initialValue;
		m_cap = m_initialCap;
	}
		

	public void Modify(float amount)
	{
		float newValue = m_Value + amount;
		m_Value = Mathf.Clamp(newValue, 0, m_cap);
	}

	public void ModifyCap(float amount)
	{
		m_cap = m_cap + amount;
		m_Value = Mathf.Clamp(m_Value, 0, m_cap);
	}
}