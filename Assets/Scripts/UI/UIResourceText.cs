using UnityEngine;
using UnityEngine.UI;

public class UIResourceText : MonoBehaviour
{
	[SerializeField] Resource m_Resource;
	[SerializeField] TMPro.TextMeshProUGUI m_ValueText;
	[SerializeField] TMPro.TextMeshProUGUI m_DifferenceText;
	[SerializeField] Color m_PositiveValueColour;
	[SerializeField] Color m_NegativeValueColour;
	[SerializeField] Color m_CapValueColour;
	[SerializeField] Color m_PositiveDifferenceColour;
	[SerializeField] Color m_NegativeDifferenceColour;
	[SerializeField] Image m_Icon;

    void Start()
    {
		Debug.Assert( m_Resource != null, gameObject );

		UpdateDisplay();
	}

    void LateUpdate()
    {
		UpdateDisplay();
	}

	void UpdateDisplay()
	{
		int value = Mathf.RoundToInt( m_Resource.m_Value * 10.0f );
		int max = Mathf.RoundToInt( m_Resource.m_Max * 10.0f );
		m_ValueText.text = value + "/" + max;
		if ( value == max && value != 0 )
		{
			m_ValueText.color = m_CapValueColour;
		}
		else if ( value > 0 )
		{
			m_ValueText.color = m_PositiveValueColour;
		}
		else
		{
			m_ValueText.color = m_NegativeValueColour;
		}
		
		m_Icon.sprite = m_Resource.m_Icon;

		int difference = Mathf.RoundToInt( m_Resource.DifferencePerTick * 10.0f );
		if ( difference > 0 )
		{
			m_DifferenceText.text = "+" + Mathf.Abs(difference);
			m_DifferenceText.color = m_PositiveDifferenceColour;
		}
		else
		{
			m_DifferenceText.text = "-" + Mathf.Abs( difference );
			m_DifferenceText.color = m_NegativeDifferenceColour;
		}
	}
}
