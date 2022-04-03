using UnityEngine;

[RequireComponent(typeof(Health))]
public class HealthVisualiser : MonoBehaviour
{
	[SerializeField] public RectTransform m_HealthPinPrefab;

	Health m_Health;
	RectTransform m_HealthUI;
	UIRadial m_HealthUI_Radial;

	private void Start()
	{
		m_Health = GetComponent<Health>();
	}

	private void OnEnable()
	{
		if ( m_HealthPinPrefab != null )
		{
			UIPinnedToWorldTransform pinManager = UIPinnedToWorldTransform.Instance;
			m_HealthUI = pinManager.InstantiateAndPin( m_HealthPinPrefab, transform );
			m_HealthUI_Radial = m_HealthUI.GetComponentInChildren<UIRadial>();
		}
	}

	private void OnDisable()
	{
		if ( m_HealthUI != null )
		{
			UIPinnedToWorldTransform pinManager = UIPinnedToWorldTransform.Instance;
			pinManager.DestroyAndRemovePin( m_HealthUI );
		}
	}

	private void Update()
	{
		if ( m_HealthUI_Radial != null )
		{
			float healthRatio = m_Health.HealthRatio;
			m_HealthUI_Radial.SetFill( healthRatio );
		}
	}
}
