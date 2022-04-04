using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowController : MonoBehaviour
{
	[SerializeField] FadeController m_FadeController;
	[SerializeField] Population m_Population;
	[Space]
	[SerializeField] FadeController m_Buildings;
	[SerializeField] FadeController m_NoPopulation;
	[SerializeField] TMPro.TextMeshProUGUI m_DurationText;
	[SerializeField] TMPro.TextMeshProUGUI m_MaxPopulationText;

	bool m_Playing;
	float m_StartTime;
	float m_EndTime;

	float m_MaximumPopulation;

	private void Start()
	{
		m_FadeController.FadeOut();

		m_Playing = true;
		m_StartTime = Time.time;
		m_EndTime = float.NaN;

		m_MaximumPopulation = 0.0f;
	}

	public void Restart()
	{
		m_FadeController.FadeIn( () => SceneManager.LoadScene( "Main" ) );
	}

	private void Update()
	{
		if ( m_Playing == false )
		{
			return;
		}

		m_MaximumPopulation = Mathf.Max( m_MaximumPopulation, m_Population.m_Value );

		if ( m_Population.m_Value > 0.0f )
		{
			return;
		}

		m_Playing = false;
		m_EndTime = Time.time;

		float durationSeconds = m_EndTime - m_StartTime;
		TimeSpan timeSpan = TimeSpan.FromSeconds( durationSeconds );
		string timeText = string.Format( "{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds );

		m_DurationText.text = timeText;
		m_MaxPopulationText.text = Mathf.RoundToInt( m_MaximumPopulation ).ToString();

		m_Buildings.FadeOut();
		m_NoPopulation.FadeIn();
	}
}
