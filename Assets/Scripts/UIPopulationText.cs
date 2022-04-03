using UnityEngine;

public class UIPopulationText : MonoBehaviour
{
	[SerializeField] Population m_pop;
	[SerializeField] TMPro.TextMeshProUGUI m_Text;

	void Start()
	{
		Debug.Assert(m_pop != null, gameObject);

		UpdateText();
	}

	void LateUpdate()
	{
		UpdateText();
	}

	void UpdateText()
	{
		m_Text.text = m_pop.m_Value + "/" + m_pop.m_cap;
	}
}
