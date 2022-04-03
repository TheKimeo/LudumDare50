using UnityEngine;
using UnityEngine.UI;

public class UIRadial : MonoBehaviour
{
	[SerializeField] Image m_RadialFill;

	public void SetFill( float ratio )
	{
		m_RadialFill.fillAmount = ratio;
	}
}
