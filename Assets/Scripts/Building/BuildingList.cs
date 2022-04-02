using UnityEngine;

[CreateAssetMenu( fileName = "BuildingList_", menuName = "Buildings/Building List", order = 0 )]
public class BuildingList : ScriptableObject
{
	[SerializeField] public BuildingType[] m_Buildings;
}
