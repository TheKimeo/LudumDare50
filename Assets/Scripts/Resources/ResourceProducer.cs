using UnityEngine;

public class ResourceProducer : MonoBehaviour
{
	[SerializeField] Resource m_ToProduce;
	[SerializeField] float m_AmountProduced;
	[SerializeField] float m_ProductionDelay;

	float m_Delay;

	private void Start()
	{
		m_Delay = m_ProductionDelay;
	}

	private void Update()
	{
		m_Delay -= Time.deltaTime;

		if (m_Delay > 0.0f )
		{
			return;
		}

		m_Delay = m_ProductionDelay;
		m_ToProduce.Modify( m_AmountProduced );
	}
}
