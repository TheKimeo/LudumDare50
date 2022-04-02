using UnityEngine;

public class UIResourceText : MonoBehaviour
{
	[SerializeField] Resource m_Resource;
	[SerializeField] TMPro.TextMeshProUGUI m_Text;

    void Start()
    {
		Debug.Assert( m_Resource != null );

		UpdateText();
	}

    void LateUpdate()
    {
		UpdateText();
	}

	void UpdateText()
	{
		m_Text.text = m_Resource.m_Value + "/" + m_Resource.m_MaxValue;
	}
}
