using UnityEngine;

public class ResourceModifier : MonoBehaviour
{
	[SerializeField] BuildingType m_buildingType;
	

	float m_tSinceProd = 0.0f;

	private void Update()
	{
		m_tSinceProd += Time.deltaTime;

		if (m_tSinceProd >= 1.0f )
		{
			foreach(BuildingType.Cost cost in m_buildingType.m_costData)
            {
				cost.m_resourceType.Modify(cost.m_runCost);
            }

			m_tSinceProd = 0.0f;
			return;
		}

	}
}
