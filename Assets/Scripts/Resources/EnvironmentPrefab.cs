using UnityEngine;

[CreateAssetMenu( fileName = "Environment_", menuName = "Environment/Environment Prefab", order = 0 )]
public class EnvironmentPrefab : ScriptableObject
{
	[SerializeField] public GameObject m_Prefab;
	[SerializeField] public float m_Weighting = 1.0f;
	[Space]
	[SerializeField] public float m_MinScale = 1.0f;
	[SerializeField] public float m_MaxScale = 1.0f;
}
