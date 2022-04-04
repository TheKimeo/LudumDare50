using UnityEngine;
using UnityEngine.UI;

public class UIBuildingSelection : MonoBehaviour
{
	[SerializeField] RectTransform m_BuildingParent;
	[SerializeField] UIBuildingButton m_BuildingButtonPrefab;
	[SerializeField] BuildingList m_Buildings;
	[SerializeField] CanvasGroup m_TooltipCanvasGroup;
	[SerializeField] float m_TooltipFadeSpeed;
	[Space]
	[SerializeField] TMPro.TextMeshProUGUI m_BuildingNameText;
	[SerializeField] TMPro.TextMeshProUGUI m_BuildingPlacementText;
	[SerializeField] GameObject m_FixedLabel;
	[SerializeField] UIBuildingResourceDisplay[] m_FixedCosts;
	[SerializeField] GameObject m_CapacityLabel;
	[SerializeField] UIBuildingResourceDisplay[] m_Capacity;
	[SerializeField] GameObject m_PerFrameLabel;
	[SerializeField] UIBuildingResourceDisplay[] m_PerFrameProduction;

	BuildingType m_HoveredBuildingType;
	BuildingType m_DisplayingBuildingType;

	private void Awake()
	{
		BuildingType[] placeableBuildings = m_Buildings.m_Buildings;
		foreach ( BuildingType buildingType in placeableBuildings )
		{
			UIBuildingButton button = GameObject.Instantiate( m_BuildingButtonPrefab, m_BuildingParent );
			button.Init( buildingType, OnPointerEnter, OnPointerExit );
		}

		m_TooltipCanvasGroup.alpha = 0.0f;
	}

	private void LateUpdate()
	{
		BuildingPlacer placer = BuildingPlacer.Instance;
		BuildingRepairer repairer = BuildingRepairer.Instance;

		//Prioritise hover target, then fall back to currently placing building
		BuildingType toDisplayBuildingType = m_HoveredBuildingType ?? placer.PlacingBuildingType ?? repairer.HoveredBuildingType;

		float targetTooltipAlpha = toDisplayBuildingType == null
			? 0.0f
			: 1.0f;

		if (m_TooltipCanvasGroup.alpha != targetTooltipAlpha)
		{
			m_TooltipCanvasGroup.alpha = Mathf.MoveTowards( m_TooltipCanvasGroup.alpha, targetTooltipAlpha, m_TooltipFadeSpeed * Time.deltaTime );
		}

		if ( m_DisplayingBuildingType != toDisplayBuildingType )
		{
			m_DisplayingBuildingType = toDisplayBuildingType;
			UpdateTooltipVisuals();
		}
	}

	void UpdateTooltipVisuals()
	{
		if ( m_DisplayingBuildingType == null )
		{
			return;
		}

		m_BuildingNameText.text = m_DisplayingBuildingType.m_UI_Name;
		m_BuildingPlacementText.text = m_DisplayingBuildingType.m_UI_Placement;

		int fixedCount = 0;
		int capacityCount = 0;
		int perFrameCount = 0;

		foreach ( BuildingType.Cost cost in m_DisplayingBuildingType.m_costData )
		{
			if ( cost.m_buildCost != 0.0f )
			{
				UIBuildingResourceDisplay display = m_FixedCosts[ fixedCount++ ];
				display.Initialize( cost );
				display.gameObject.SetActive( true );
			}

			if ( cost.m_capIncrease != 0.0f )
			{
				UIBuildingResourceDisplay display = m_Capacity[ capacityCount++ ];
				display.Initialize( cost ); //TODO: Display something else
				display.gameObject.SetActive( true );
			}

			if ( cost.m_productionPerTick != 0.0f )
			{
				UIBuildingResourceDisplay display = m_PerFrameProduction[ perFrameCount++ ];
				display.Initialize( cost ); //TODO: Display something else
				display.gameObject.SetActive( true );
			}
		}

		m_FixedLabel.SetActive( fixedCount > 0 );
		for ( int i = fixedCount; i < m_FixedCosts.Length; ++i )
		{
			UIBuildingResourceDisplay display = m_FixedCosts[ i ];
			display.gameObject.SetActive( false );
		}

		m_CapacityLabel.SetActive( capacityCount > 0 );
		for ( int i = capacityCount; i < m_Capacity.Length; ++i )
		{
			UIBuildingResourceDisplay display = m_Capacity[ i ];
			display.gameObject.SetActive( false );
		}

		m_PerFrameLabel.SetActive( perFrameCount > 0 );
		for ( int i = perFrameCount; i < m_PerFrameProduction.Length; ++i )
		{
			UIBuildingResourceDisplay display = m_PerFrameProduction[ i ];
			display.gameObject.SetActive( false );
		}

		LayoutRebuilder.ForceRebuildLayoutImmediate( m_TooltipCanvasGroup.GetComponent<RectTransform>() );
	}

	void OnPointerEnter(BuildingType building)
	{
		m_HoveredBuildingType = building;
	}

	void OnPointerExit(BuildingType building)
	{
		Debug.Assert( m_HoveredBuildingType == building );
		m_HoveredBuildingType = null;
	}
}
