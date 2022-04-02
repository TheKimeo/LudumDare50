using UnityEngine;

[CreateAssetMenu(fileName = "BuildingType", menuName = "Buildings/BuildingType", order = 0)]
public class BuildingType : ScriptableObject
{
	[System.Serializable]
	public class Cost
    {
		public Resource m_resourceType;
		public float m_buildCost;
		public float m_runCost;
    }

	[SerializeField] public GameObject m_ghost;
	[SerializeField] public GameObject m_real;
	[SerializeField] public Sprite m_UI_Icon;
	[SerializeField] public Cost[] m_costData;


}
