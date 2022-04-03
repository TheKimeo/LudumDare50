using UnityEngine;

public class ResourceModifier : MonoBehaviour
{
	[SerializeField] BuildingType m_buildingType;
	[SerializeField] BuildingState m_buildingState;
	[SerializeField] Population m_population;

	float m_tSinceProd = 0.0f;


	private void OnEnable()
	{
		foreach (BuildingType.Cost cost in m_buildingType.m_costData)
		{
			cost.m_resourceType.m_MaxValue += cost.m_capIncrease;
		}

		m_population.m_cap += m_buildingType.m_popCapIncrease;

	}

    private void OnDisable()
    {	
		foreach (BuildingType.Cost cost in m_buildingType.m_costData)
		{
			cost.m_resourceType.m_MaxValue -= cost.m_capIncrease;
		}

		m_population.m_cap -= m_buildingType.m_popCapIncrease;

	}

	private void Update()
	{
		m_tSinceProd += Time.deltaTime;

		if ( m_tSinceProd >= 1.0f )
		{
			foreach ( BuildingType.Cost cost in m_buildingType.m_costData )
			{
				float modifyAmount = cost.m_runCost;
				if ( modifyAmount > 0.0f )
				{
					//Only positive generation is effected by operational shutdown
					modifyAmount *= m_buildingState.OperationalRatio;
				}

				cost.m_resourceType.Modify( modifyAmount );
			}

			m_tSinceProd = 0.0f;
			return;
		}
	}
}
