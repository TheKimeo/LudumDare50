using UnityEngine;
using UnityEngine.UI;

public class UIBuildingResourceDisplay : MonoBehaviour
{
	public enum DisplayStat
	{
		Cost,
		Capacity,
		Production,
	}

	[SerializeField] DisplayStat m_DisplayStat;
	[SerializeField] Image m_Icon;
	[SerializeField] TMPro.TextMeshProUGUI m_ValueText;
	[SerializeField] Color m_NeutralColour;
	[SerializeField] Color m_PositiveColour;
	[SerializeField] Color m_NegativeColour;

	BuildingType.Cost m_BuildingCost;

	public void Initialize( BuildingType.Cost buildingCost )
	{
		m_BuildingCost = buildingCost;

		UpdateDisplay();
	}

	void LateUpdate()
	{
		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		if (m_BuildingCost == null)
		{
			return;
		}

		float gameValue = GetValue();
		int uiValue = Mathf.RoundToInt( gameValue * 10.0f );

		m_ValueText.text = uiValue.ToString();

		switch ( m_DisplayStat )
		{
			case DisplayStat.Cost:
				bool canAfford = m_BuildingCost.m_resourceType.CanConsume( m_BuildingCost.m_buildCost );

				if ( canAfford )
				{
					m_ValueText.color = m_NeutralColour;
				}
				else
				{
					m_ValueText.color = m_NegativeColour;
				}
				break;
			case DisplayStat.Capacity:
				m_ValueText.color = m_NeutralColour;
				break;
			case DisplayStat.Production:
				m_ValueText.color = uiValue > 0.0f ? m_PositiveColour : m_NegativeColour;
				break;
		}
		

		m_Icon.sprite = m_BuildingCost.m_resourceType.m_Icon;
	}

	float GetValue()
	{
		switch ( m_DisplayStat )
		{
			case DisplayStat.Cost:
				return m_BuildingCost.m_buildCost;
			case DisplayStat.Capacity:
				return m_BuildingCost.m_capIncrease;
			case DisplayStat.Production:
				return m_BuildingCost.m_productionPerTick;
		}

		Debug.Assert( false );
		return 0.0f;
	}
}
