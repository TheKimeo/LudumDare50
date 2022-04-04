using UnityEngine;
using UnityEngine.Events;

public class FadeController : MonoBehaviour
{
	[SerializeField] CanvasGroup m_Fade;
	[SerializeField] float m_FadeDuration = 0.5f;
	[SerializeField] float m_InitialAlpha = 1.0f;
	[SerializeField] bool m_DisableRaycastsAtZeroAlpha = true;

	float m_FadeStartTime;
	float m_AlphaTarget;
	UnityAction m_OnComplete;

	private void Awake()
	{
		m_Fade.alpha = m_InitialAlpha;
		m_AlphaTarget = m_InitialAlpha;
	}

	private void Update()
	{
		if ( m_DisableRaycastsAtZeroAlpha )
		{
			bool blockRaycasts = m_Fade.alpha > 0.0f;
			if ( m_Fade.blocksRaycasts != blockRaycasts )
			{
				m_Fade.blocksRaycasts = blockRaycasts;
			}
		}

		if ( m_Fade.alpha == m_AlphaTarget )
		{
			return;
		}

		float timePassed = Time.time - m_FadeStartTime;
		float ratio = Mathf.Clamp01( timePassed / m_FadeDuration );
		m_Fade.alpha = Mathf.Lerp( 1.0f - m_AlphaTarget, m_AlphaTarget, ratio );

		if ( timePassed >= m_FadeDuration )
		{
			m_OnComplete?.Invoke();
		}
	}

	public void FadeIn( UnityAction onComplete = null )
	{
		m_AlphaTarget = 1.0f;
		m_Fade.alpha = 0.0f;
		m_FadeStartTime = Time.time;
		m_OnComplete = onComplete;
	}

	public void FadeOut( UnityAction onComplete = null )
	{
		m_AlphaTarget = 0.0f;
		m_Fade.alpha = 1.0f;
		m_FadeStartTime = Time.time;
		m_OnComplete = onComplete;
	}
}
