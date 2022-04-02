using UnityEngine;

[CreateAssetMenu(fileName = "BuildingType", menuName = "Buildings/BuildingType", order = 0)]
public class BuildingType : ScriptableObject
{
	[SerializeField] public GameObject m_ghost;
	[SerializeField] public GameObject m_real;
	[SerializeField] public Sprite m_UI_Icon;

}
