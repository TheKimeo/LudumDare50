using UnityEngine;
using UnityEngine.UI;

public class UIBuildingButton : MonoBehaviour
{
	[SerializeField] Image m_Icon;
	[SerializeField] Button m_Button;

	BuildingType m_BuildingType;

	public void Init(BuildingType buildingType)
	{
		m_BuildingType = buildingType;
		m_Icon.sprite = buildingType.m_UI_Icon;

		m_Button.onClick.RemoveListener( OnButtonClicked );
		m_Button.onClick.AddListener( OnButtonClicked );
	}

	void OnButtonClicked()
	{
		BuildingPlacer buildingPlacer = BuildingPlacer.Instance;
		buildingPlacer.SetType( m_BuildingType );
		InputModeManager.Instance.SetMode(InputModeManager.Mode.BUILD);
	}
}
