using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontendFlowController : MonoBehaviour
{
	[SerializeField] FadeController m_FadeController;

	private void Start()
	{
		m_FadeController.FadeOut();
	}

	public void Play()
	{
		m_FadeController.FadeIn( () => SceneManager.LoadScene( "Main" ) );
	}

	public void ToggleMute()
	{
		//TODO
	}

	public void Exit()
	{
		Application.Quit();
	}
}
