using UnityEngine;
using UnityEngine.Events;

public class WorldButton : MonoBehaviour
{
	[SerializeField] public UnityEvent m_OnMouseClick = new UnityEvent();
	[SerializeField] public UnityEvent m_OnMouseHover = new UnityEvent();
	[SerializeField] public UnityEvent m_OnMouseUnHover = new UnityEvent();
}
