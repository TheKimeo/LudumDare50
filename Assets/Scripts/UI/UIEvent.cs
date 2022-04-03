using UnityEngine;
using UnityEngine.UI;

public class UIEvent : MonoBehaviour
{
	[SerializeField] Image m_Icon;
	
	public void Initialize( EventManager.Event e )
	{
		m_Icon.sprite = e.m_Behaviour.m_EventIcon;
	}
}
