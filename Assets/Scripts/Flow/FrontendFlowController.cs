using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontendFlowController : MonoBehaviour
{
	[SerializeField] FadeController m_FadeController;

	private void Start()
	{
		m_FadeController.FadeOutBlack();
	}

	public void Play()
	{
		m_FadeController.FadeInBlack( () => SceneManager.LoadScene( "Main" ) );
	}

	public void ToggleMute()
	{
		if(AudioListener.volume > 0)
        {
			AudioListener.volume = 0;
		}
		else
        {
			AudioListener.volume = 1;
		}
	}

	public void Exit()
	{
		Application.Quit();
	}
}
