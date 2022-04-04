using UnityEngine;

public class ResourceModifier : MonoBehaviour, ResourceManager.IResourceModifier
{
	[SerializeField] BuildingType m_buildingType;
	[SerializeField] BuildingState m_buildingState;
	[SerializeField] Population m_population;
	[SerializeField] FloatReference m_popResBoost;


	private void OnEnable()
	{
		ResourceManager resourceManager = ResourceManager.Instance;
		resourceManager.m_OnTickEvent.Add( this );

		foreach ( BuildingType.Cost cost in m_buildingType.m_costData )
		{
			cost.m_resourceType.ModifyCap( cost.m_capIncrease );
		}

		m_population.ModifyCap( m_buildingType.m_popCapIncrease );
	}

	private void OnDisable()
	{
		ResourceManager resourceManager = ResourceManager.Instance;
		if ( resourceManager != null )
		{
			resourceManager.m_OnTickEvent.Remove( this );
		}

		foreach ( BuildingType.Cost cost in m_buildingType.m_costData )
		{
			cost.m_resourceType.ModifyCap( -cost.m_capIncrease );
		}

		m_population.ModifyCap( -m_buildingType.m_popCapIncrease );

	}

	void ResourceManager.IResourceModifier.OnGainTick( Resource resource, ref float currentCumulative )
	{
		BuildingType.Cost cost = GetMatchingCost( resource );
		if ( cost == null )
		{
			return;
		}

		float modifyAmount = cost.m_productionPerTick;
		if ( modifyAmount <= 0.0f )
		{
			return;
		}

		modifyAmount *= m_population.m_Value * m_popResBoost.Value + 1.0f;
		modifyAmount *= m_buildingState.OperationalRatio;

		currentCumulative += modifyAmount;
	}

	void ResourceManager.IResourceModifier.OnLossTick( Resource resource, ref float currentCumulative )
	{
		BuildingType.Cost cost = GetMatchingCost( resource );
		if ( cost == null )
		{
			return;
		}

		float modifyAmount = cost.m_productionPerTick;
		if ( modifyAmount >= 0.0f )
		{
			return;
		}

		if ( ( resource.m_Value + currentCumulative ) < -modifyAmount )
		{
			m_buildingState.AddSuppressor( this );
		}
		else
		{
			m_buildingState.RemoveSuppressor( this );
		}

		currentCumulative += modifyAmount;
	}

	BuildingType.Cost GetMatchingCost( Resource resource )
	{
		foreach ( BuildingType.Cost cost in m_buildingType.m_costData )
		{
			if ( cost.m_resourceType == resource )
			{
				return cost;
			}
		}

		return null;
	}
}
