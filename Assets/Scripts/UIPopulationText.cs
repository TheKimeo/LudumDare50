using UnityEngine;
using UnityEngine.UI;

public class UIPopulationText : MonoBehaviour
{
	[SerializeField] Population m_pop;
	[SerializeField] FloatReference m_popResourceIncrease;
	[SerializeField] TMPro.TextMeshProUGUI m_Text;
	[SerializeField] TMPro.TextMeshProUGUI m_MultiplierText;
	[SerializeField] Color m_PositiveValueColour;
	[SerializeField] Color m_ZeroValueColour;
	[SerializeField] Color m_CapValueColour;
	[SerializeField] Color m_PositiveDifferenceColour;
	[SerializeField] Color m_NegativeDifferenceColour;
	[SerializeField] Image m_Icon;

	void Start()
	{
		Debug.Assert(m_pop != null, gameObject);

		UpdateDisplay();
	}

	void LateUpdate()
	{
		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		int value = Mathf.RoundToInt( m_pop.m_Value );
		int cap = Mathf.RoundToInt( m_pop.m_cap );

		m_Icon.sprite = m_pop.m_Icon;

		m_Text.text = value + "/" + cap;
		if (value == cap && value != 0)
		{
			m_Text.color = m_CapValueColour;
		}
		else if ( value > 0 )
		{
			m_Text.color = m_PositiveValueColour;
		}
		else
		{
			m_Text.color = m_NegativeDifferenceColour;
		}

		int multiplier = Mathf.RoundToInt( m_pop.m_Value * m_popResourceIncrease.Value * 100.0f );
		if (multiplier >= 0)
		{
			m_MultiplierText.text = "+" + multiplier + "%";
			m_MultiplierText.color = m_PositiveDifferenceColour;
		}
		else
		{
			m_MultiplierText.text = "-" + multiplier + "%";
			m_MultiplierText.color = m_NegativeDifferenceColour;
		}
	}
}
