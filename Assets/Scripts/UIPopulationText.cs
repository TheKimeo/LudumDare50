using UnityEngine;
using UnityEngine.UI;

public class UIPopulationText : MonoBehaviour
{
	[SerializeField] Population m_pop;
	[SerializeField] TMPro.TextMeshProUGUI m_Text;
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
		m_Text.text = m_pop.m_Value + "/" + m_pop.m_cap;
		m_Icon.sprite = m_pop.m_Icon;
	}
}
