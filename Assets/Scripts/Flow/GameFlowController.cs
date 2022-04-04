using UnityEngine;

public class GameFlowController : MonoBehaviour
{
	[SerializeField] FadeController m_FadeController;

	private void Start()
	{
		m_FadeController.FadeOutBlack();
	}
}
