using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIBuildingButton : MonoBehaviour
{
	[SerializeField] Image m_Icon;
	[SerializeField] Button m_Button;

	BuildingType m_BuildingType;

	UnityAction<BuildingType> m_OnPointerEnter;
	UnityAction<BuildingType> m_OnPointerExit;

	public void Init(BuildingType buildingType, UnityAction<BuildingType> onPointerEnter, UnityAction<BuildingType> onPointerExit)
	{
		m_BuildingType = buildingType;
		m_Icon.sprite = buildingType.m_UI_Icon;

		m_Button.onClick.RemoveListener( OnButtonClicked );
		m_Button.onClick.AddListener( OnButtonClicked );

		m_OnPointerEnter = onPointerEnter;
		m_OnPointerExit = onPointerExit;
	}

	void OnButtonClicked()
	{
		BuildingPlacer buildingPlacer = BuildingPlacer.Instance;
		buildingPlacer.SetType( m_BuildingType );
		InputModeManager.Instance.SetMode(InputModeManager.Mode.BUILD);
	}

	public void OnPointerEnter()
	{
		m_OnPointerEnter.Invoke( m_BuildingType );
	}

	public void OnPointerExit()
	{
		m_OnPointerExit.Invoke( m_BuildingType );
	}
}
