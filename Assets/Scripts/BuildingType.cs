using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "BuildingType", menuName = "Buildings/BuildingType", order = 0)]
public class BuildingType : ScriptableObject
{
	[System.Serializable]
	public class Cost
    {
		public Resource m_resourceType;
		public float m_buildCost;
		[FormerlySerializedAs("m_runCost")] public float m_productionPerTick;
		public float m_capIncrease;
    }


	[SerializeField] public float m_popCapIncrease = 0;

	[SerializeField] public GameObject m_ghost;
	[SerializeField] public GameObject m_real;
	[SerializeField] public string m_snapLayer = "";
	[SerializeField] public Sprite m_UI_Icon;
	[SerializeField] public string m_UI_Name = "[placeholder]";
	[SerializeField] public string m_UI_Placement = "Anywhere";
	[SerializeField] public Cost[] m_costData;


}
