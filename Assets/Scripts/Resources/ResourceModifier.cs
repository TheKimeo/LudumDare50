using UnityEngine;

public class ResourceModifier : MonoBehaviour
{
	[SerializeField] BuildingType m_buildingType;
	[SerializeField] BuildingState m_buildingState;
	[SerializeField] Population m_population;

	private void OnEnable()
	{
		ResourceManager resourceManager = ResourceManager.Instance;
		resourceManager.m_OnTickEvent += OnResourceTick;

		foreach (BuildingType.Cost cost in m_buildingType.m_costData)
		{
			cost.m_resourceType.m_Max += cost.m_capIncrease;
		}

		m_population.m_cap += m_buildingType.m_popCapIncrease;

	}

    private void OnDisable()
    {
		ResourceManager resourceManager = ResourceManager.Instance;
		if ( resourceManager != null )
		{
			resourceManager.m_OnTickEvent -= OnResourceTick;
		}

		foreach (BuildingType.Cost cost in m_buildingType.m_costData)
		{
			cost.m_resourceType.m_Max -= cost.m_capIncrease;
		}

		m_population.m_cap -= m_buildingType.m_popCapIncrease;

	}

	void OnResourceTick()
	{
		foreach ( BuildingType.Cost cost in m_buildingType.m_costData )
		{
			float modifyAmount = cost.m_productionPerTick;
			if ( modifyAmount > 0.0f )
			{
				//Only positive generation is effected by operational shutdown
				modifyAmount *= m_buildingState.OperationalRatio;
			}

			cost.m_resourceType.Modify( modifyAmount );
		}
	}
}
