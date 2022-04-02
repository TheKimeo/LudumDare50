using UnityEngine;

public class UIBuildingSelection : MonoBehaviour
{
	[SerializeField] RectTransform m_BuildingParent;
	[SerializeField] UIBuildingButton m_BuildingButtonPrefab;
	[SerializeField] BuildingList m_Buildings;

	private void Awake()
	{
		BuildingType[] placeableBuildings = m_Buildings.m_Buildings;
		foreach ( BuildingType buildingType in placeableBuildings )
		{
			UIBuildingButton button = GameObject.Instantiate( m_BuildingButtonPrefab, m_BuildingParent );
			button.Init( buildingType );
		}
	}
}
