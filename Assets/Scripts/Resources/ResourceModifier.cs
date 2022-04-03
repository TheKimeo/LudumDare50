using UnityEngine;

public class ResourceModifier : MonoBehaviour
{
	[SerializeField] BuildingType m_buildingType;
	[SerializeField] BuildingState m_buildingState;
	[SerializeField] Population m_population;
	[SerializeField] FloatReference m_popResBoost;


	private void OnEnable()
	{
		ResourceManager resourceManager = ResourceManager.Instance;
		resourceManager.m_OnTickEvent += OnResourceTick;

		foreach (BuildingType.Cost cost in m_buildingType.m_costData)
		{
			cost.m_resourceType.ModifyCap(cost.m_capIncrease);
		}

		m_population.ModifyCap(m_buildingType.m_popCapIncrease);

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
			cost.m_resourceType.ModifyCap(-cost.m_capIncrease);
		}

		m_population.ModifyCap(-m_buildingType.m_popCapIncrease);

	}

	void OnResourceTick()
	{
		foreach ( BuildingType.Cost cost in m_buildingType.m_costData )
		{
			float modifyAmount = cost.m_productionPerTick;
			if ( modifyAmount > 0.0f )
			{
				modifyAmount += cost.m_productionPerTick * m_population.m_initialValue * m_popResBoost.Value;
				Debug.Log("Increase by " + cost.m_productionPerTick * m_population.m_initialValue * m_popResBoost.Value);
				//Only positive generation is effected by operational shutdown
				modifyAmount *= m_buildingState.OperationalRatio;
			}
			else
            {
				if (!cost.m_resourceType.CanConsume(-modifyAmount))
                {
					m_buildingState.AddSuppressor(this);
				}
				else
                {
					m_buildingState.RemoveSuppressor(this);
				}
			}


			cost.m_resourceType.Modify( modifyAmount );
		}
	}
}
