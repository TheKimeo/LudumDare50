using UnityEngine;
using UnityEngine.UI;

public class UIResourceText : MonoBehaviour
{
	[SerializeField] Resource m_Resource;
	[SerializeField] TMPro.TextMeshProUGUI m_Text;
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
		m_Text.text = (int)m_Resource.m_Value + "/" + m_Resource.m_Max;
		m_Icon.sprite = m_Resource.m_Icon;
	}
}
