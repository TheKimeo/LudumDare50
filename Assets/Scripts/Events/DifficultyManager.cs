using UnityEngine;

public class DifficultyManager : Singleton<DifficultyManager>
{
	[SerializeField] float m_InitialDifficulty = 1.0f;
	[SerializeField] float m_DifficultyCycleMultiplier = 1.0f;

	public float Difficulty => m_InitialDifficulty + TimeManager.Instance.CycleTime * m_DifficultyCycleMultiplier;
}
